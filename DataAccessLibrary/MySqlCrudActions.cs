using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Models;
using DataAccessLibrary.Models.BusinessData;
using DataAccessLibrary.Models.ClientData;
using DataAccessLibrary.Models.OwnerData;
using DataAccessLibrary.Models.ProgramData;
using DataAccessLibrary.Models.SharedData;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Utilities.Collections;

namespace DataAccessLibrary
{
    public class MySqlCrudActions
    {
        private readonly string _connectionString;
        private MySqlDataAccess db = new MySqlDataAccess();

        public MySqlCrudActions(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE ACTIONS
        // TODO: Test changing return type to ints for create functions
        // TODO: Logic: Create will still create even if returned int isn't passed through
        public int CreateClientFile(ClientFile clientFile) //chris
        {
            string sql = "INSERT INTO client (business_name, first_name, last_name) " + //insert into client table of mysql for these columns field names, 
                         "VALUES (@business_name, @first_name, @last_name);"; //All this is the command for the server at the end made into a string.

            // if client exist
            // save idclient to int idclient
            // GetOwnerFileById(idclient)

            db.SaveData(sql,
                new { clientFile.business_name, clientFile.first_name, clientFile.last_name },
                _connectionString);

            sql = "SELECT idclient " +
                  "FROM client " +
                  "WHERE idclient = (SELECT LAST_INSERT_ID());";

            int idClient = db.LoadData<Client, dynamic>(sql,
                new { },
                _connectionString).First().idclient;

            // Context for below: Zipcodes cannot be repeated
            // Check if zipcode passed in from model already exists; don't add it if it does
            int idZipcode;
            try
            {
                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<Zipcode, dynamic>(sql,
                    new { clientFile.zip },
                    _connectionString).First().idzipcode;
            }
            catch (InvalidOperationException)
            {
                idZipcode = 0;
            }

            Trace.WriteLine(idZipcode);

            if (idZipcode == 0)
            {
                sql = "INSERT INTO zipcode (zip) " +
                      "VALUES (@zip);";

                db.SaveData(sql,
                    new { clientFile.zip },
                    _connectionString);

                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<Zipcode, dynamic>(sql,
                    new { clientFile.zip },
                    _connectionString).First().idzipcode;
            }

            // BIG LEARNING ISSUE UNDERNEATH FOR CP
            // Context for below: States cannot be repeated
            // Check if state passed in from model already exists; don't add it if it does
            // TODO: Constrain this to 50 states
            int idState;
            try
            {
                sql = "SELECT idstate " +
                      "FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<State, dynamic>(sql,
                    new { clientFile.state_code },
                    _connectionString).First().idstate;
            }
            catch (InvalidOperationException)
            {
                idState = 0;
            }


            if (idState == 0)
            {
                sql = "INSERT INTO state (state_code) " +
                      "VALUES (@state_code);";

                db.SaveData(sql,
                    new { clientFile.state_code },
                    _connectionString);

                sql = "SELECT idstate FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<State, dynamic>(sql,
                    new { clientFile.state_code },
                    _connectionString).First().idstate;
            }

            // Context for below: County names can be used twice just not within the same state
            // Check if county exists in state passed in from model; don't add it if it does
            int idCounty;
            try
            {
                sql = "SELECT idcounty " +
                      "FROM county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<County, dynamic>(sql,
                    new { clientFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;

            }
            catch (InvalidOperationException)
            {
                idCounty = 0;
            }

            if (idCounty == 0)
            {
                sql = "INSERT INTO county (state_idstate, county_name, county_region) " +
                      "VALUES (@state_idstate, @county_name, @county_region);";

                db.SaveData(sql,
                        new { state_idstate = idState, clientFile.county_name, clientFile.county_region },
                        _connectionString);

                sql = "SELECT idcounty from county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<County, dynamic>(sql,
                    new { clientFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;
            }

            // Context for below: City names can be used twice just not within the same county
            // Check if city exists in county passed in from model; don't add it if it does
            // TODO: Check if city is in county that is in California; If so require city code
            int idCity;
            try
            {
                sql = "SELECT idcity " +
                      "FROM city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<City, dynamic>(sql,
                    new { clientFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }
            catch (InvalidOperationException)
            {
                idCity = 0;
            }

            if (idCity == 0)
            {
                sql = "INSERT INTO city (county_idcounty, city_name, city_code) " +
                      "VALUES (@county_idcounty, @city_name, @city_code);";

                db.SaveData(sql,
                        new { county_idcounty = idCounty, clientFile.city_name, clientFile.city_code },
                        _connectionString);

                sql = "SELECT idcity from city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<City, dynamic>(sql,
                    new { clientFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }
            // Save address info to database
            sql = "INSERT INTO client_address (client_idclient, address_line1_number, address_line1_street_name, address_line2, city_idcity, zipcode_idzipcode) " +
                  "VALUES (@client_idclient, @address_line1_number, @address_line1_street_name, @address_line2, @city_idcity, @zipcode_idzipcode);";

            db.SaveData(sql,
                new { client_idclient = idClient, clientFile.address_line1_number, clientFile.address_line1_street_name, clientFile.address_line2, city_idcity = idCity, zipcode_idzipcode = idZipcode },
                _connectionString);

            sql = "INSERT INTO client_phone (client_idclient, area_code, phone_number) " +
                  "VALUES (@client_idclient, @area_code, @phone_number);";

            db.SaveData(sql,
                new { client_idclient = idClient, clientFile.area_code, clientFile.phone_number },
                _connectionString);
            return idClient;
        }
        public int CreateBusinessFile(BusinessFile businessFile)
        {
            // Save basic owner info and create pk for entry
            string sql = "INSERT INTO business (dba) " +
                         "VALUES (@dba);";

            db.SaveData(sql,
                new { businessFile.dba },
                _connectionString);

            sql = "SELECT idbusiness " +
                  "FROM business " +
                  "WHERE idbusiness = (SELECT LAST_INSERT_ID());";

            int idBusiness = db.LoadData<BusinessFile, dynamic>(sql,
                new { },
                _connectionString).First().idbusiness;

            // Context for below: Zipcodes cannot be repeated
            // Check if zipcode passed in from model already exists; don't add it if it does
            int idZipcode;
            try
            {
                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.zip },
                    _connectionString).First().idzipcode;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO zipcode (zip) " +
                      "VALUES (@zip);";

                db.SaveData(sql,
                    new { businessFile.zip },
                    _connectionString);

                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.zip },
                    _connectionString).First().idzipcode;
            }

            // Context for below: States cannot be repeated
            // Check if state passed in from model already exists; don't add it if it does
            // TODO: Constrain this to 50 states
            int idState;
            try
            {
                sql = "SELECT idstate " +
                      "FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.state_code },
                    _connectionString).First().idstate;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO state (state_code) " +
                      "VALUES (@state_code);";

                db.SaveData(sql,
                    new { businessFile.state_code },
                    _connectionString);

                sql = "SELECT idstate FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.state_code },
                    _connectionString).First().idstate;
            }

            // Context for below: County names can be used twice just not within the same state
            // Check if county exists in state passed in from model; don't add it if it does
            int idCounty;
            try
            {
                sql = "SELECT idcounty " +
                      "FROM county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;

            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO county (state_idstate, county_name, county_region) " +
                      "VALUES (@state_idstate, @county_name, @county_region);";

                db.SaveData(sql,
                    new { state_idstate = idState, businessFile.county_name, businessFile.county_region },
                    _connectionString);

                sql = "SELECT idcounty from county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;
            }

            // Context for below: City names can be used twice just not within the same county
            // Check if city exists in county passed in from model; don't add it if it does
            // TODO: Check if city is in county that is in California; If so require city code
            int idCity;
            try
            {
                sql = "SELECT idcity " +
                      "FROM city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO city (county_idcounty, city_name, city_code) " +
                      "VALUES (@county_idcounty, @city_name, @city_code);";

                db.SaveData(sql,
                    new { county_idcounty = idCounty, businessFile.city_name, businessFile.city_code },
                    _connectionString);

                sql = "SELECT idcity FROM city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }

            // Process activity codes
            foreach (var activityCode in businessFile.business_activity_codes)
            {
                try
                {
                    sql = "SELECT idactivity_code FROM business_activity_code " +
                          "WHERE activity_code_name = @activity_code_name;";

                    activityCode.idactivity_code = db.LoadData<BusinessActivityCode, dynamic>(sql,
                        new { activityCode.activity_code_name },
                        _connectionString).First().idactivity_code;

                }
                catch (InvalidOperationException)
                {
                    sql = "INSERT INTO business_activity_code (activity_code_name) " +
                          "VALUES (@activity_code_name);";

                    db.SaveData(sql, new { activityCode.activity_code_name },
                        _connectionString);

                    sql = "SELECT idactivity_code FROM business_activity_code " +
                          "WHERE activity_code_name = @activity_code_name;";

                    activityCode.idactivity_code = db.LoadData<BusinessActivityCode, dynamic>(sql,
                        new { activityCode.activity_code_name },
                        _connectionString).First().idactivity_code;
                }
                //Trace.WriteLine($"Code name: {activityCode.activity_code_name} Code ID: {activityCode.idactivity_code}");
            }

            // Save business info to database
            sql = "INSERT INTO business_address (business_idbusiness, address_line1_number, address_line1_street_name, address_line2, city_idcity, zipcode_idzipcode) " +
                  "VALUES (@business_idbusiness, @address_line1_number, @address_line1_street_name, @address_line2, @city_idcity, @zipcode_idzipcode);";

            db.SaveData(sql,
                new { business_idbusiness = idBusiness, businessFile.address_line1_number, businessFile.address_line1_street_name, businessFile.address_line2, city_idcity = idCity, zipcode_idzipcode = idZipcode },
                _connectionString);

            sql = "INSERT INTO business_phone (business_idbusiness, area_code, phone_number) " +
                  "VALUES (@business_idbusiness, @area_code, @phone_number);";

            db.SaveData(sql,
                new { business_idbusiness = idBusiness, businessFile.area_code, businessFile.phone_number },
                _connectionString);

            sql = "INSERT INTO business_license (business_idbusiness, license_number, establishment, entity, active, activity_date) " +
                  "VALUES (@business_idbusiness, @license_number, @establishment, @entity, @active, @activity_date);";

            db.SaveData(sql,
                new { business_idbusiness = idBusiness, businessFile.license_number, businessFile.establishment, businessFile.entity, businessFile.active, businessFile.activity_date },
                _connectionString);

            // purposely leaving out user_iduser until that system is implemented
            sql = "INSERT INTO business_interaction (business_idbusiness, worked_date, completed_date) " +
                  "VALUES (@business_idbusiness, @worked_date, @completed_date);";

            db.SaveData(sql,
                new { business_idbusiness = idBusiness, businessFile.worked_date, businessFile.completed_date },
                _connectionString);

            sql = "INSERT INTO business_memo (business_idbusiness, memo_text) " +
                  "VALUES (@business_idbusiness, @memo_text);";

            db.SaveData(sql,
                new { business_idbusiness = idBusiness, businessFile.memo_text },
                _connectionString);

            // Relationship tables

            // Relate activity codes to business licenses
            // Get id of business_license for this business and store it in idbusiness_license property

            sql = "SELECT idbusiness_license FROM business_license " +
                  "WHERE business_idbusiness = @idbusiness;";

            businessFile.idbusiness_license = db.LoadData<BusinessFile, dynamic>(sql,
                new { idBusiness },
                _connectionString).First().idbusiness_license;
            //Trace.WriteLine($"businessfile id = {businessFile.idbusiness_license}");

            // Add entry for each activity code
            foreach (var activityCode in businessFile.business_activity_codes)
            {
                sql = "INSERT INTO business_license_has_business_activity_code " +
                      "VALUES (@business_license_idbusiness_license, @business_activity_code_idactivity_code);";

                db.SaveData(sql, new { business_license_idbusiness_license = businessFile.idbusiness_license, business_activity_code_idactivity_code = activityCode.idactivity_code },
                    _connectionString);
            }

            return idBusiness;
        }
        public int CreateOwnerFile(OwnerFile ownerFile)
        {
            // Check if owner exists up here using inner join and info passed in from file
            // Save basic owner info and create pk for entry
            string sql = "INSERT INTO owner (first_name, last_name, socsec) " +
                         "VALUES (@first_name, @last_name, @socsec);";

            // if owner exist
            // save ownerid to int ownerid
            // GetOwnerFileById(ownerid)

            db.SaveData(sql,
            new { ownerFile.first_name, ownerFile.last_name, ownerFile.socsec },
                    _connectionString);

            sql = "SELECT idowner " +
                  "FROM owner " +
                  "WHERE idowner = (SELECT LAST_INSERT_ID());";

            int idOwner = db.LoadData<OwnerFile, dynamic>(sql,
                new { },
                _connectionString).First().idowner;

            // Explanation on try catch blocks below: If no idzipcode is found for the passed in zip c# attempts to assign nothing to int idZipcode; This is an invalid operation and
            // thus triggers the exception. We catch this exception and insert the passed in(determined now to be new) zip into the DB.

            // Context for below: Zipcodes cannot be repeated
            // Check if zipcode passed in from model already exists; don't add it if it does
            int idZipcode;
            try
            {
                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.zip },
                    _connectionString).First().idzipcode;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO zipcode (zip) " +
                      "VALUES (@zip);";

                db.SaveData(sql,
                    new { ownerFile.zip },
                    _connectionString);

                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.zip },
                    _connectionString).First().idzipcode;
            }

            // Context for below: States cannot be repeated
            // Check if state passed in from model already exists; don't add it if it does
            // TODO: Constrain this to 50 states
            int idState;
            try
            {
                sql = "SELECT idstate " +
                      "FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.state_code },
                    _connectionString).First().idstate;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO state (state_code) " +
                      "VALUES (@state_code);";

                db.SaveData(sql,
                    new { ownerFile.state_code },
                    _connectionString);

                sql = "SELECT idstate FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.state_code },
                    _connectionString).First().idstate;
            }

            // Context for below: County names can be used twice just not within the same state
            // Check if county exists in state passed in from model; don't add it if it does
            int idCounty;
            try
            {
                sql = "SELECT idcounty " +
                      "FROM county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;

            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO county (state_idstate, county_name, county_region) " +
                      "VALUES (@state_idstate, @county_name, @county_region);";

                db.SaveData(sql,
                    new { state_idstate = idState, ownerFile.county_name, ownerFile.county_region },
                    _connectionString);

                sql = "SELECT idcounty from county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;
            }

            // Context for below: City names can be used twice just not within the same county
            // Check if city exists in county passed in from model; don't add it if it does
            // TODO: Check if city is in county that is in California; If so require city code
            int idCity;
            try
            {
                sql = "SELECT idcity " +
                      "FROM city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO city (county_idcounty, city_name, city_code) " +
                      "VALUES (@county_idcounty, @city_name, @city_code);";

                db.SaveData(sql,
                    new { county_idcounty = idCounty, ownerFile.city_name, ownerFile.city_code },
                    _connectionString);

                sql = "SELECT idcity from city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }

            // Save address info to database
            sql = "INSERT INTO owner_address (owner_idowner, address_line1_number, address_line1_street_name, address_line2, city_idcity, zipcode_idzipcode) " +
                  "VALUES (@owner_idowner, @address_line1_number, @address_line1_street_name, @address_line2, @city_idcity, @zipcode_idzipcode);";

            db.SaveData(sql,
                new { owner_idowner = idOwner, ownerFile.address_line1_number, ownerFile.address_line1_street_name, ownerFile.address_line2, city_idcity = idCity, zipcode_idzipcode = idZipcode },
                _connectionString);

            sql = "INSERT INTO owner_phone (owner_idowner, area_code, phone_number) " +
                  "VALUES (@owner_idowner, @area_code, @phone_number);";

            db.SaveData(sql,
                new { owner_idowner = idOwner, ownerFile.area_code, ownerFile.phone_number },
                _connectionString);

            sql = "INSERT INTO owner_position (owner_idowner, title) " +
                  "VALUES (@owner_idowner, @title);";

            db.SaveData(sql,
                new { owner_idowner = idOwner, ownerFile.title },
                _connectionString);

            sql = "INSERT INTO owner_timeline (owner_idowner, stock, from_date, to_date) " +
                  "VALUES (@owner_idowner, @stock, @from_date, @to_date);";

            db.SaveData(sql,
                new { owner_idowner = idOwner, ownerFile.stock, ownerFile.from_date, ownerFile.to_date },
                _connectionString);

            sql = "INSERT INTO owner_blind (owner_idowner, blind_text) " +
                  "VALUES (@owner_idowner, @blind_text);";

            db.SaveData(sql,
                new { owner_idowner = idOwner, ownerFile.blind_text },
                _connectionString);
            return idOwner;
        }
        public void CreateGeneralFile(int businessFileId, List<int>ownerFileIds , List<int>clientFileIds)
        {

            string sql = "SELECT idbusiness_interaction " +
                         "FROM business_interaction " +
                         "WHERE business_idbusiness = @idbusiness;";

            int businessInteractionId =
                db.LoadData<BusinessInteractionIdLookupModel, dynamic>(sql, new { idbusiness = businessFileId }, _connectionString).First().idbusiness_interaction;

            foreach (var ownerFileId in ownerFileIds)
            {
                sql = "INSERT INTO business_has_owner " + 
                      "VALUES (@business_idbusiness, @owner_idowner);";

                db.SaveData(sql,
                    new { business_idbusiness = businessFileId, owner_idowner = ownerFileId },
                    _connectionString);
            }

            foreach (var clientFileId in clientFileIds)
            {
                sql = "INSERT INTO business_interaction_has_client " +
                      "VALUES (@business_interaction_idbusiness_interaction, @client_idclient);";

                db.SaveData(sql,
                    new { business_interaction_idbusiness_interaction = businessInteractionId, client_idclient = clientFileId }, _connectionString);
            }
        }
        public void AddBusinessToClient(int businessInteractionFileId, int clientFileId)
        {
            string sql = "INSERT INTO business_interaction_has_client " +
                         "VALUES (@business_interaction_idbusiness_interaction, @client_idclient);";

            db.SaveData(sql, 
                new { business_interaction_idbusiness_interaction = businessInteractionFileId, client_idclient = clientFileId },
                _connectionString);
        }

        // READ ACTIONS
        public BusinessFile GetBusinessFileById(int idBusiness)
        {
            BusinessFile output = new BusinessFile();

            string sql = "SELECT idbusiness, dba, " +
                         "address_line1_number, address_line1_street_name, address_line2, " +
                         "city_name, city_code, " +
                         "county_name, county_region, " +
                         "state_code, " +
                         "zip, " +
                         "area_code, phone_number, " +
                         "license_number, establishment, entity, active, activity_date, " +
                         "worked_date, completed_date, " +
                         "memo_text " +
                         "FROM(((((((((business " +
                         "LEFT JOIN business_address ON business.idbusiness = business_address.business_idbusiness) " +
                         "LEFT JOIN city ON business_address.city_idcity = city.idcity) " +
                         "LEFT JOIN county ON city.county_idcounty = county.idcounty) " +
                         "LEFT JOIN state ON county.state_idstate = state.idstate) " +
                         "LEFT JOIN zipcode ON business_address.zipcode_idzipcode = zipcode.idzipcode) " +
                         "LEFT JOIN business_phone ON business.idbusiness = business_phone.business_idbusiness) " +
                         "LEFT JOIN business_license ON business.idbusiness = business_license.business_idbusiness) " +
                         "LEFT JOIN business_interaction ON business.idbusiness = business_interaction.business_idbusiness) " +
                         "LEFT JOIN business_memo ON business.idbusiness = business_memo.business_idbusiness) " +
                         "WHERE business.idbusiness = @idbusiness;";

            output = db.LoadData<BusinessFile, dynamic>(sql,
                new { idbusiness = idBusiness },
                _connectionString).First();


            sql = "SELECT business_idbusiness_old " +
                  "FROM business_formerly " +
                  "WHERE business_idbusiness = @idbusiness;";

            output.business_formerlies = db.LoadData<BusinessFormerly, dynamic>(sql,
                new { idbusiness = idBusiness },
                _connectionString);


            sql = "SELECT activity_code_name " +
                  "FROM(((business " +
                  "INNER JOIN business_license ON business.idbusiness = business_license.business_idbusiness) " +
                  "INNER JOIN business_license_has_business_activity_code ON business_license.business_idbusiness = business_license_idbusiness_license) " +
                  "INNER JOIN business_activity_code ON idactivity_code = business_activity_code_idactivity_code) " +
                  "WHERE business.idbusiness = @idbusiness;";

            output.business_activity_codes = db.LoadData<BusinessActivityCode, dynamic>(sql,
                new { idbusiness = idBusiness },
                _connectionString);

            return output;
        }
        public int LastInsertedBusiness()
        {
            string sql = "SELECT idbusiness " +
                         "FROM business " +
                         "WHERE idbusiness = (SELECT LAST_INSERT_ID());";

            int idBusiness = db.LoadData<BusinessFile, dynamic>(sql,
                new { },
                _connectionString).First().idbusiness;
            return idBusiness;
        }
        public List<BusinessIdLookupModel> SearchBusinessFile(BusinessFile businessFile)
        {
            string sql = "SELECT idbusiness " +
                         "FROM (((((((business " +
                         "LEFT JOIN business_address ON business.idbusiness = business_address.business_idbusiness) " +
                         "LEFT JOIN city ON business_address.city_idcity = city.idcity) " +
                         "LEFT JOIN county ON city.county_idcounty = county.idcounty) " +
                         "LEFT JOIN state ON county.state_idstate = state.idstate) " +
                         "LEFT JOIN zipcode ON business_address.zipcode_idzipcode = zipcode.idzipcode) " +
                         "LEFT JOIN business_phone ON business.idbusiness = business_phone.business_idbusiness) " +
                         "LEFT JOIN business_license ON business.idbusiness = business_license.business_idbusiness) " +
                         "WHERE ((dba = @dba) OR (@dba IS NULL)) " +
                         "AND ((address_line1_number = @address_line1_number) OR (@address_line1_number IS NULL)) " +
                         "AND ((address_line1_street_name = @address_line1_street_name) OR (@address_line1_street_name IS NULL)) " +
                         "AND ((address_line2 = @address_line2) OR (@address_line2 IS NULL)) " +
                         "AND ((city_name = @city_name) OR (@city_name IS NULL)) " +
                         "AND ((county_name = @county_name) OR (@county_name IS NULL)) " +
                         "AND ((state_code = @state_code) OR (@state_code IS NULL)) " +
                         "AND ((zip = @zip) OR (@zip IS NULL)) " +
                         "AND ((area_code = @area_code) OR (@area_code IS NULL)) " +
                         "AND ((phone_number = @phone_number) OR (@phone_number IS NULL)) " +
                         "AND ((license_number = @license_number) OR (@license_number IS NULL)) " +
                         "AND ((establishment = @establishment) OR (@establishment IS NULL)) " +
                         "AND ((entity = @entity) OR (@entity IS NULL)) " +
                         "AND ((active = @active) OR (@active IS NULL));";

            return db.LoadData<BusinessIdLookupModel, dynamic>(sql, new
            {
                businessFile.dba,
                businessFile.address_line1_number,
                businessFile.address_line1_street_name,
                businessFile.address_line2,
                businessFile.city_name,
                businessFile.county_name,
                businessFile.state_code,
                businessFile.zip,
                businessFile.area_code,
                businessFile.phone_number,
                businessFile.license_number,
                businessFile.establishment,
                businessFile.entity,
                businessFile.active
            }, 
                _connectionString);

        }
        public OwnerFile GetOwnerFileById(int idOwner)
        {
            OwnerFile output = new OwnerFile();

            try
            {
                string sql =
                    "SELECT idowner, first_name, last_name, socsec, " +
                    "address_line1_number, address_line1_street_name, address_line2, " +
                    "city_name, city_code, " +
                    "county_name, county_region, " +
                    "state_code, " +
                    "zip, " +
                    "area_code, phone_number, " +
                    "title, " +
                    "stock, from_date, to_date, " +
                    "blind_text " +
                    "FROM(((((((((owner " +
                    "LEFT JOIN owner_address ON owner.idowner = owner_address.owner_idowner) " +
                    "LEFT JOIN city ON owner_address.city_idcity = city.idcity) " +
                    "LEFT JOIN county ON city.county_idcounty = county.idcounty) " +
                    "LEFT JOIN state ON county.state_idstate = state.idstate) " +
                    "LEFT JOIN zipcode ON owner_address.zipcode_idzipcode = zipcode.idzipcode) " +
                    "LEFT JOIN owner_phone ON owner.idowner = owner_phone.owner_idowner) " +
                    "LEFT JOIN owner_position ON owner.idowner = owner_position.owner_idowner) " +
                    "LEFT JOIN owner_timeline ON owner.idowner = owner_timeline.owner_idowner) " +
                    "LEFT JOIN owner_blind ON owner.idowner = owner_blind.owner_idowner) " +
                    "WHERE owner.idowner = @idowner;";

                output = db.LoadData<OwnerFile, dynamic>(sql,
                    new { idowner = idOwner },
                    _connectionString).First();
                return output;
            }
            catch (InvalidOperationException e)
            {
                Trace.WriteLine(e + $"\nOwner: {idOwner} does not exist in the database.");
                throw;
            }
        }
        public int LastInsertedOwner()
        {
            string sql = "SELECT idowner " +
                         "FROM owner " +
                         "WHERE idowner = (SELECT LAST_INSERT_ID());";

            int idOwner = db.LoadData<OwnerFile, dynamic>(sql,
                new { },
                _connectionString).First().idowner;
            return idOwner;
        }
        public List<OwnerIdLookupModel> SearchOwnerFile(OwnerFile ownerFile)
        {
            string sql = "SELECT idowner " +
                         "FROM ((((((((owner " +
                         "LEFT JOIN owner_address ON owner.idowner = owner_address.owner_idowner) " +
                         "LEFT JOIN city ON owner_address.city_idcity = city.idcity) " +
                         "LEFT JOIN county ON city.county_idcounty = county.idcounty) " +
                         "LEFT JOIN state ON county.state_idstate = state.idstate) " +
                         "LEFT JOIN zipcode ON owner_address.zipcode_idzipcode = zipcode.idzipcode) " +
                         "LEFT JOIN owner_phone ON owner.idowner = owner_phone.owner_idowner) " +
                         "LEFT JOIN owner_position ON owner.idowner = owner_position.owner_idowner) " +
                         "LEFT JOIN owner_blind ON owner.idowner = owner_blind.owner_idowner) " +
                         "WHERE ((first_name = @first_name) OR (@first_name IS NULL)) " +
                         "AND ((last_name = @last_name) OR (@last_name IS NULL)) " +
                         "AND ((address_line1_number = @address_line1_number) OR (@address_line1_number IS NULL)) " +
                         "AND ((address_line1_street_name = @address_line1_street_name) OR (@address_line1_street_name IS NULL)) " +
                         "AND ((address_line2 = @address_line2) OR (@address_line2 IS NULL)) " +
                         "AND ((city_name = @city_name) OR (@city_name IS NULL)) " +
                         "AND ((county_name = @county_name) OR (@county_name IS NULL)) " +
                         "AND ((state_code = @state_code) OR (@state_code IS NULL)) " +
                         "AND ((zip = @zip) OR (@zip IS NULL)) " +
                         "AND ((area_code = @area_code) OR (@area_code IS NULL)) " +
                         "AND ((phone_number = @phone_number) OR (@phone_number IS NULL)) " +
                         "AND ((title = @title) OR (@title IS NULL)) " +
                         "AND ((blind_text = @blind_text) or (@blind_text IS NULL));";

            return db.LoadData<OwnerIdLookupModel, dynamic>(sql, new
            {
                ownerFile.first_name,
                ownerFile.last_name,
                ownerFile.address_line1_number,
                ownerFile.address_line1_street_name,
                ownerFile.address_line2,
                ownerFile.city_name,
                ownerFile.county_name,
                ownerFile.state_code,
                ownerFile.zip,
                ownerFile.area_code,
                ownerFile.phone_number,
                ownerFile.title,
                ownerFile.blind_text
            },
                _connectionString);
        }
        public ClientFile GetClientFileById(int idClient)
        {
            ClientFile output = new ClientFile();
            try
            {
                string sql =
                    "SELECT idclient, business_name, first_name, last_name, " +
                    "address_line1_number, address_line1_street_name, address_line2, " +
                    "city_name, city_code, " +
                    "county_name, county_region, " +
                    "state_code, " +
                    "zip, " +
                    "area_code, phone_number " +
                    "FROM ((((((client " +
                    "LEFT JOIN client_address ON client.idclient = client_address.client_idclient) " +
                    "LEFT JOIN city ON client_address.city_idcity = city.idcity) " +
                    "LEFT JOIN county ON city.county_idcounty = county.idcounty) " +
                    "LEFT JOIN state ON county.state_idstate = state.idstate) " +
                    "LEFT JOIN zipcode ON client_address.zipcode_idzipcode = zipcode.idzipcode) " +
                    "LEFT JOIN client_phone ON client.idclient = client_phone.client_idclient) " +
                    "WHERE client.idclient = @idclient;";

                output = db.LoadData<ClientFile, dynamic>(sql,
                    new { idclient = idClient },
                    _connectionString).First();

                return output;
            }
            catch (InvalidOperationException e)
            {
                Trace.WriteLine(e + $"\nClient: {idClient} does not exist in the database.");
                throw;
            }
        }
        public int LastInsertedClient()
        {
            string sql = "SELECT idclient " +
                  "FROM client " +
                  "WHERE idclient = (SELECT LAST_INSERT_ID());";

            int idClient = db.LoadData<Client, dynamic>(sql,
                new { },
                _connectionString).First().idclient;
            return idClient;
        }
        public List<ClientIdLookupModel> SearchClientFile(ClientFile clientFile)
        {
            string sql = "SELECT idclient " +
                         "FROM ((((((client " +
                         "LEFT JOIN client_address ON client.idclient = client_address.client_idclient) " +
                         "LEFT JOIN city ON client_address.city_idcity = city.idcity) " +
                         "LEFT JOIN county ON city.county_idcounty = county.idcounty) " +
                         "LEFT JOIN state ON county.state_idstate = state.idstate) " +
                         "LEFT JOIN zipcode ON client_address.zipcode_idzipcode = zipcode.idzipcode) " +
                         "LEFT JOIN client_phone ON client.idclient = client_phone.client_idclient) " +
                         "WHERE ((business_name = @business_name) OR (@business_name IS NULL)) " +
                         "AND ((first_name = @first_name) OR (@first_name IS NULL)) " +
                         "AND ((last_name = @last_name) OR (@last_name IS NULL)) " +
                         "AND ((address_line1_number = @address_line1_number) OR (@address_line1_number IS NULL)) " +
                         "AND ((address_line1_street_name = @address_line1_street_name) OR (@address_line1_street_name IS NULL)) " +
                         "AND ((address_line2 = @address_line2) OR (@address_line2 IS NULL)) " +
                         "AND ((city_name = @city_name) OR (@city_name IS NULL)) " +
                         "AND ((county_name = @county_name) OR (@county_name IS NULL)) " +
                         "AND ((state_code = @state_code) OR (@state_code IS NULL)) " +
                         "AND ((zip = @zip) OR (@zip IS NULL)) " +
                         "AND ((area_code = @area_code) OR (@area_code IS NULL)) " +
                         "AND ((phone_number = @phone_number) OR (@phone_number IS NULL));";

            return db.LoadData<ClientIdLookupModel, dynamic>(sql, new
                {
                    clientFile.business_name,
                    clientFile.first_name,
                    clientFile.last_name,
                    clientFile.address_line1_number,
                    clientFile.address_line1_street_name,
                    clientFile.address_line2,
                    clientFile.city_name,
                    clientFile.county_name,
                    clientFile.state_code,
                    clientFile.zip,
                    clientFile.area_code,
                    clientFile.phone_number
                },
                _connectionString);
        }
        public GeneralFile GetGeneralFileById(int? idBusiness = null, int? idOwner = null, int? idClient = null)
        {
            GeneralFile generalFile = new GeneralFile();
            string sql = "";

            if (idBusiness != null)
            {
                // Take all owners
                sql = "SELECT owner_idowner " +
                      "FROM business_has_owner " +
                      "WHERE business_idbusiness = @idbusiness;";

                var owners = db.LoadData<int, dynamic>(sql, new
                    {
                        idBusiness
                    },
                    _connectionString);

                // Take all clients
                sql = "SELECT client_idclient " +
                      "FROM business_interaction_has_client " +
                      "WHERE business_interaction_idbusiness_interaction = @idbusiness;";

                var clients = db.LoadData<int, dynamic>(sql, new
                    {
                        idBusiness
                    },
                    _connectionString);
                //Trace.WriteLine($"BusId: {business.idbusiness} OwnersCount: {owners.Count} ClientsCount: {clients.Count}");

                generalFile.idbusiness = (int)idBusiness; // TODO: Lookup better way to do this; Casting to not null seems silly
                generalFile.owner_idowner = owners;
                generalFile.client_idclient = clients;
            }

            return generalFile;
        }
        public List<GeneralFile> SearchGeneralFile(ClientFile? clientFile = null, BusinessFile? businessFile = null, OwnerFile? ownerFile = null)
        {
            List<GeneralFile> generalFiles = new List<GeneralFile>();

            string sql = "";

            if (businessFile != null)
            {
                var businesses = SearchBusinessFile(businessFile);
                foreach (var business in businesses)
                {
                    // Take all owners
                        sql = "SELECT owner_idowner " +
                              "FROM business_has_owner " +
                              "WHERE business_idbusiness = @idbusiness;";

                        var owners = db.LoadData<int, dynamic>(sql, new
                            {
                                business.idbusiness
                            },
                            _connectionString);
                        
                    // Take all clients
                    sql = "SELECT client_idclient " +
                          "FROM business_interaction_has_client " +
                          "WHERE business_interaction_idbusiness_interaction = @idbusiness;";

                    var clients = db.LoadData<int, dynamic>(sql, new
                        {
                            business.idbusiness
                        },
                        _connectionString);
                    Trace.WriteLine($"BusId: {business.idbusiness} OwnersCount: {owners.Count} ClientsCount: {clients.Count}");
                    generalFiles.Add(new GeneralFile() {idbusiness = business.idbusiness, owner_idowner = owners, client_idclient = clients});

                }

                // For debug; Prints all owner files and client files related to this business file
                //var ownerList = "";
                //var clientList = "";
                //foreach (var generalfile in generalFiles)
                //{
                //    foreach (var owner in generalfile.owner_idowner)
                //    {
                //        ownerList += $" {owner.ToString()}";
                //    }

                //    foreach (var client in generalfile.client_idclient)
                //    {
                //        clientList += $" {client.ToString()}";
                //    }

                //    var printString = $"BusinessID: {generalfile.idbusiness} Owners: {ownerList} Clients: {clientList}";
                //    Trace.WriteLine(printString);
                //    ownerList = "";
                //    clientList = "";
                //}
                
                return generalFiles;
            }

            if (ownerFile != null)
            {
                var owners = SearchOwnerFile(ownerFile);
                foreach (var owner in owners)
                {
                    sql = "SELECT business_idbusiness, owner_idowner, client_idclient " +
                          "FROM business_has_owner " +
                          "LEFT JOIN business_interaction_has_client ON business.idbusiness = business_interaction_has_client.business_interaction_idbusiness_interaction " +
                          "WHERE business_idbusiness = @idbusiness;";

                    generalFiles = db.LoadData<GeneralFile, dynamic>(sql, new
                        {
                            owner.idowner
                        },
                        _connectionString);
                }
                return generalFiles;
            }

            if (clientFile != null)
            {
                var clients = SearchClientFile(clientFile);
                foreach (var client in clients)
                {
                    sql = "SELECT business_idbusiness, owner_idowner, client_idclient " +
                          "FROM business_has_owner " +
                          "LEFT JOIN business_interaction_has_client ON business.idbusiness = business_interaction_has_client.business_interaction_idbusiness_interaction " +
                          "WHERE business_idbusiness = @idbusiness;";

                    generalFiles = db.LoadData<GeneralFile, dynamic>(sql, new
                        {
                            client.idclient
                        },
                        _connectionString);
                }
                return generalFiles;
            }
            return generalFiles;
        }

        // UPDATE ACTIONS
        public void UpdateBusinessFile(BusinessFile businessFile)
        {
            // Context for below: Zipcodes cannot be repeated
            // Check if zipcode passed in from model already exists; don't add it if it does
            string sql;
            int idZipcode;
            try
            {
                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.zip },
                    _connectionString).First().idzipcode;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO zipcode (zip) " +
                      "VALUES (@zip);";

                db.SaveData(sql,
                    new { businessFile.zip },
                    _connectionString);

                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.zip },
                    _connectionString).First().idzipcode;
            }

            // Context for below: States cannot be repeated
            // Check if state passed in from model already exists; don't add it if it does
            // TODO: Constrain this to 50 states
            int idState;
            try
            {
                sql = "SELECT idstate " +
                      "FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.state_code },
                    _connectionString).First().idstate;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO state (state_code) " +
                      "VALUES (@state_code);";

                db.SaveData(sql,
                    new { businessFile.state_code },
                    _connectionString);

                sql = "SELECT idstate FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.state_code },
                    _connectionString).First().idstate;
            }

            // Context for below: County names can be used twice just not within the same state
            // Check if county exists in state passed in from model; don't add it if it does
            int idCounty;
            try
            {
                sql = "SELECT idcounty " +
                      "FROM county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;

            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO county (state_idstate, county_name, county_region) " +
                      "VALUES (@state_idstate, @county_name, @county_region);";

                db.SaveData(sql,
                    new { state_idstate = idState, businessFile.county_name, businessFile.county_region },
                    _connectionString);

                sql = "SELECT idcounty from county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;
            }

            // Context for below: City names can be used twice just not within the same county
            // Check if city exists in county passed in from model; don't add it if it does
            // TODO: Check if city is in county that is in California; If so require city code
            int idCity;
            try
            {
                sql = "SELECT idcity " +
                      "FROM city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO city (county_idcounty, city_name, city_code) " +
                      "VALUES (@county_idcounty, @city_name, @city_code);";

                db.SaveData(sql,
                    new { county_idcounty = idCounty, businessFile.city_name, businessFile.city_code },
                    _connectionString);

                sql = "SELECT idcity FROM city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<BusinessFile, dynamic>(sql,
                    new { businessFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }

            // Process activity codes
            foreach (var activityCode in businessFile.business_activity_codes)
            {
                try
                {
                    sql = "SELECT idactivity_code FROM business_activity_code " +
                          "WHERE activity_code_name = @activity_code_name;";

                    activityCode.idactivity_code = db.LoadData<BusinessActivityCode, dynamic>(sql,
                        new { activityCode.activity_code_name },
                        _connectionString).First().idactivity_code;

                }
                catch (InvalidOperationException)
                {
                    sql = "INSERT INTO business_activity_code (activity_code_name) " +
                          "VALUES (@activity_code_name);";

                    db.SaveData(sql, new { activityCode.activity_code_name },
                        _connectionString);

                    sql = "SELECT idactivity_code FROM business_activity_code " +
                          "WHERE activity_code_name = @activity_code_name;";

                    activityCode.idactivity_code = db.LoadData<BusinessActivityCode, dynamic>(sql,
                        new { activityCode.activity_code_name },
                        _connectionString).First().idactivity_code;
                }
                //Trace.WriteLine($"Code name: {activityCode.activity_code_name} Code ID: {activityCode.idactivity_code}");
            }

            // Relationship tables

            // Relate activity codes to business licenses
            // Get id of business_license for this business and store it in idbusiness_license property

            // DELETE THEN INSERT TO FIX
            // DELETE ANY ROWS THAT MATCH IDBUSINESS
            // RE-ADD ROWS WITH UPDATED KEYS

            sql = "SELECT idbusiness_license FROM business_license " +
                  "WHERE business_idbusiness = @idbusiness;";

            businessFile.idbusiness_license = db.LoadData<BusinessFile, dynamic>(sql,
                new { businessFile.idbusiness },
                _connectionString).First().idbusiness_license;
            //Trace.WriteLine($"businessfile id = {businessFile.idbusiness_license}");

            // Add entry for each activity code
            // TODO: Turn this insert into an update
            foreach (var activityCode in businessFile.business_activity_codes)
            {
                sql = "INSERT INTO business_license_has_business_activity_code " +
                      "VALUES (@business_license_idbusiness_license, @business_activity_code_idactivity_code);";

                db.SaveData(sql, new { business_license_idbusiness_license = businessFile.idbusiness_license, business_activity_code_idactivity_code = activityCode.idactivity_code },
                    _connectionString);
            }



        }
        public void UpdateOwnerFile(OwnerFile ownerFile)
        {

            // Context for below: Zipcodes cannot be repeated
            // Check if zipcode passed in from model already exists; don't add it if it does
            string sql;
            int idZipcode;
            try
            {
                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.zip },
                    _connectionString).First().idzipcode;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO zipcode (zip) " +
                      "VALUES (@zip);";

                db.SaveData(sql,
                    new { ownerFile.zip },
                    _connectionString);

                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.zip },
                    _connectionString).First().idzipcode;
                Trace.WriteLine("Exception in zip ran");
            }

            // Context for below: States cannot be repeated
            // Check if state passed in from model already exists; don't add it if it does
            // TODO: Constrain this to 50 states
            int idState;
            try
            {
                sql = "SELECT idstate " +
                      "FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.state_code },
                    _connectionString).First().idstate;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO state (state_code) " +
                      "VALUES (@state_code);";

                db.SaveData(sql,
                    new { ownerFile.state_code },
                    _connectionString);

                sql = "SELECT idstate FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.state_code },
                    _connectionString).First().idstate;
            }


            // Context for below: County names can be used twice just not within the same state
            // Check if county exists in state passed in from model; don't add it if it does
            int idCounty;
            try
            {
                sql = "SELECT idcounty " +
                      "FROM county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;

            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO county (state_idstate, county_name, county_region) " +
                      "VALUES (@state_idstate, @county_name, @county_region);";

                db.SaveData(sql,
                    new { state_idstate = idState, ownerFile.county_name, ownerFile.county_region },
                    _connectionString);

                sql = "SELECT idcounty from county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;
            }

            // Context for below: City names can be used twice just not within the same county
            // Check if city exists in county passed in from model; don't add it if it does
            // TODO: Check if city is in county that is in California; If so require city code
            int idCity;
            try
            {
                sql = "SELECT idcity " +
                      "FROM city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO city (county_idcounty, city_name, city_code) " +
                      "VALUES (@county_idcounty, @city_name, @city_code);";

                db.SaveData(sql,
                    new { county_idcounty = idCounty, ownerFile.city_name, ownerFile.city_code },
                    _connectionString);

                sql = "SELECT idcity from city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<OwnerFile, dynamic>(sql,
                    new { ownerFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }

            //Trace.WriteLine("CITY ID" + idCity);

            sql =
                "UPDATE owner " +
                "LEFT JOIN owner_address ON owner_address.owner_idowner = idowner " +
                "LEFT JOIN owner_phone ON owner_phone.owner_idowner = idowner " +
                "LEFT JOIN owner_position ON owner_position.owner_idowner = idowner " +
                "LEFT JOIN owner_timeline ON owner_timeline.owner_idowner = idowner " +
                "LEFT JOIN owner_blind ON owner_blind.owner_idowner = idowner " +
                "SET first_name = @first_name, last_name = @last_name, " +
                "address_line1_number = @address_line1_number, address_line1_street_name = @address_line1_street_name, address_line2 = @address_line2, city_idcity = @city_idcity, zipcode_idzipcode = @zipcode_idzipcode, " +
                "area_code = @area_code, phone_number = @phone_number, " +
                "title = @title, " +
                "stock = @stock, from_date = @from_date, to_date = @to_date, " +
                "blind_text = @blind_text " +
                "WHERE idowner = @idowner;";

             db.SaveData(sql,
                 new
                 {
                     ownerFile.idowner, ownerFile.first_name, ownerFile.last_name, 
                     ownerFile.address_line1_number, ownerFile.address_line1_street_name, ownerFile.address_line2, city_idcity = idCity, zipcode_idzipcode = idZipcode,
                     ownerFile.area_code, ownerFile.phone_number, 
                     ownerFile.title,
                     ownerFile.stock, ownerFile.from_date, ownerFile.to_date,
                     ownerFile.blind_text
                 }, _connectionString);

        }
        public void UpdateClientFile(ClientFile clientFile)
        {
            string sql;
            int idZipcode;
            try
            {
                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<ClientFile, dynamic>(sql,
                    new { clientFile.zip },
                    _connectionString).First().idzipcode;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO zipcode (zip) " +
                      "VALUES (@zip);";

                db.SaveData(sql, new { clientFile.zip },
                    _connectionString);

                sql = "SELECT idzipcode " +
                      "FROM zipcode " +
                      "WHERE zip = @zip;";

                idZipcode = db.LoadData<ClientFile, dynamic>(sql,
                    new { clientFile.zip },
                    _connectionString).First().idzipcode;
            }
            // Context for below: States cannot be repeated
            // Check if state passed in from model already exists; don't add it if it does
            // TODO: Constrain this to 50 states
            int idState;
            try
            {
                sql = "SELECT idstate " +
                      "FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<ClientFile, dynamic>(sql,
                    new { clientFile.state_code },
                    _connectionString).First().idstate;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO state (state_code) " +
                      "VALUES (@state_code);";

                db.SaveData(sql,
                    new { clientFile.state_code },
                    _connectionString);

                sql = "SELECT idstate FROM state " +
                      "WHERE state_code = @state_code;";

                idState = db.LoadData<ClientFile, dynamic>(sql,
                    new { clientFile.state_code },
                    _connectionString).First().idstate;
            }


            // Context for below: County names can be used twice just not within the same state
            // Check if county exists in state passed in from model; don't add it if it does
            int idCounty;
            try
            {
                sql = "SELECT idcounty " +
                      "FROM county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<ClientFile, dynamic>(sql,
                    new { clientFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;

            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO county (state_idstate, county_name, county_region) " +
                      "VALUES (@state_idstate, @county_name, @county_region);";

                db.SaveData(sql,
                    new { state_idstate = idState, clientFile.county_name, clientFile.county_region },
                    _connectionString);

                sql = "SELECT idcounty from county " +
                      "WHERE county_name = @county_name " +
                      "AND state_idstate = @state_idstate;";

                idCounty = db.LoadData<ClientFile, dynamic>(sql,
                    new { clientFile.county_name, state_idstate = idState },
                    _connectionString).First().idcounty;
            }

            // Context for below: City names can be used twice just not within the same county
            // Check if city exists in county passed in from model; don't add it if it does
            // TODO: Check if city is in county that is in California; If so require city code
            int idCity;
            try
            {
                sql = "SELECT idcity " +
                      "FROM city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<ClientFile, dynamic>(sql,
                    new { clientFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }
            catch (InvalidOperationException)
            {
                sql = "INSERT INTO city (county_idcounty, city_name, city_code) " +
                      "VALUES (@county_idcounty, @city_name, @city_code);";

                db.SaveData(sql,
                    new { county_idcounty = idCounty, clientFile.city_name, clientFile.city_code },
                    _connectionString);

                sql = "SELECT idcity from city " +
                      "WHERE city_name = @city_name " +
                      "AND county_idcounty = @idcounty;";

                idCity = db.LoadData<ClientFile, dynamic>(sql,
                    new { clientFile.city_name, idcounty = idCounty },
                    _connectionString).First().idcity;
            }
            // TODO: Create function to check for existing city, zip, etc. then insert
            sql =
                "UPDATE client " +
                "LEFT JOIN client_phone ON client_phone.client_idclient = idclient " +
                "LEFT JOIN client_address ON client_address.client_idclient = idclient " +
                "SET business_name = @business_name, first_name = @first_name, last_name = @last_name, " +
                "area_code = @area_code, phone_number = @phone_number, " +
                "address_line1_number = @address_line1_number, address_line1_street_name, address_line2 = @address_line2, city_idcity = @city_idcity, zipcode_idzipcode = @zipcode_idzipcode " +
                "WHERE idclient = @idclient;";

            db.SaveData(sql,
            new
            {
                clientFile.idclient, clientFile.business_name, clientFile.first_name, clientFile.last_name, 
                clientFile.address_line1_number, clientFile.address_line1_street_name, clientFile.address_line2, city_idcity = idCity, zipcode_idzipcode = idZipcode, 
                clientFile.area_code, clientFile.phone_number
            }, _connectionString);
        }
        public void UpdateGeneralFile(int businessFileId, List<int> ownerFileIds, List<int> clientFileIds)
        {
            string sql = "UPDATE business_has_owner " +
                         "WHERE business_idbusiness = @businessFileId " +
                         "AND owner_idowner = @ownerFileId " +
                         "SET ";
        }

        // DELETE ACTIONS
    }
}
