using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkilledWorkersService.Model;
using SkilledWorkersService.Models;
using UserManagementService.Models;

namespace SkilledWorkersService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserManagementController : Controller
    {
        // GET: api/UserManagement
        private readonly _dbContext _context;
        public UserManagementController(_dbContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<object> Get()
        {
            var data = _context.Users.ToList();
            return data;
        }

        // GET: api/UserManagement/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<object> Get(int id)
        {
            var data = _context.Users.ToList();
            return data;
        }

        // POST: api/UserManagement
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/UserManagement/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPut]
        [ActionName("LoginPortal")]
        public async Task<object> LoginPortal(LoginModel _user)
        {
            try
            {
                object getuserDetails = new object();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    string encrypted_password = Encrypt(_user.password);

                    var getuser = _context.Users.Where(x => x.username == _user.username && x.password == encrypted_password).FirstOrDefault();
                    if (getuser != null)
                    {
                        if (getuser.user_type == 1)
                        {
                             getuserDetails = _context.CustomerProfile.Where(x => x.user_id == getuser.id).FirstOrDefault();
                        }
                        else
                        {
                             getuserDetails = _context.ProviderProfile.Where(x => x.user_id == getuser.id).FirstOrDefault();
                        }
                    }
                    
                    if (getuser != null)
                    {
                        var modl = new
                        {
                            statuscode = 200,
                            message = "Login Successfully",
                            data = getuser,
                            user_details= getuserDetails
                        };
                        return Ok(modl);
                        
                    }
                    else
                    {
                        var modl = new
                        {
                            statuscode = 201,
                            message = "Login failed",
                            data = getuser
                        };
                        return Unauthorized(modl);
                    }
                    
                }
                    
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, ex.Message);
                return BadRequest("App User Login Failed. Reason: " + ex.Message);
                //return BadRequest(ex.Message);
            }
        }

        
        [HttpPut]
        [ActionName("RegisterUser")]
        public async Task<object> RegisterUser(RegisterModel _user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {




                    /// add login Details///

                    string encrypted_password = Encrypt(_user.password); 
                    Users login_model = new Users();
                    login_model.user_type = _user.user_type;
                    login_model.username = _user.username;
                    login_model.password = encrypted_password;
                    login_model.email = _user.email;

                    _context.Users.Add(login_model);
                    _context.SaveChanges();
                    if (_user.user_type == 1)
                    {
                        CustomerProfile model = new CustomerProfile();
                        model.user_id = login_model.id;
                        model.first_name = _user.first_name;
                        model.last_name = _user.last_name;
                        model.created_at = System.DateTime.Now;
                        model.email = _user.email;
                        _context.CustomerProfile.Add(model);
                        _context.SaveChanges();

                    }
                    else
                    {
                        ProviderProfile model_p = new ProviderProfile();
                        model_p.user_id = login_model.id;
                        model_p.first_name = _user.first_name;
                        model_p.last_name = _user.last_name;
                        model_p.phone_1 = _user.phone_num.ToString();
                        model_p.phone_1 = _user.phone_num;
                        model_p.created_at = System.DateTime.Now;
                        model_p.email = _user.email;
                        model_p.category_id = Convert.ToInt32(_user.category);
                        model_p.sub_category_id = Convert.ToInt32(_user.sub_category) ;
                        model_p.company_name = _user.company_name;
                        model_p.premium_user = Convert.ToInt32(_user.package)==1?true:false;
                        _context.ProviderProfile.Add(model_p);
                        _context.SaveChanges();
                    }

                        var modl = new
                        {
                            statuscode = 200,
                            message = "Register Successfully",
                            //data = getuser
                        };
                        return Ok(modl);

                    

                }

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, ex.Message);
                var modl = new
                {
                    statuscode = 200,
                    message = "Register failed",
                };
                return Unauthorized(modl);
                //return BadRequest("App User Login Failed. Reason: " + ex.Message);
                //return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("getUserProfileById")]
        public async Task<object> getUserProfileById(int user_id,int user_type)
        {
            try
            {
                if (user_type == 1)
                {
                    var user_details = _context.CustomerProfile.Where(x => x.user_id == user_id).FirstOrDefault();
                    var geCitiesList = await _context.Cities.Where(x => x.status == true).Select(y => new { y.id, y.title, y.country_id }).ToListAsync();
                    var modl = new
                    {
                        statuscode = 200,
                        data = user_details,
                        cities = geCitiesList,
                    };
                    return Ok(modl);
                }
                else
                {
                    var user_details = _context.ProviderProfile.Where(x => x.user_id == user_id).FirstOrDefault();
                    var geCitiesList = await _context.Cities.Where(x => x.status == true).Select(y => new { y.id, y.title, y.country_id }).ToListAsync();
                    var modl = new
                    {
                        statuscode = 200,
                        data = user_details,
                        cities = geCitiesList,
                    };
                    return Ok(modl);
                }

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, ex.Message);
                var modl = new
                {
                    statuscode = 200,
                    message = " not found",
                };
                return Unauthorized(modl);
                //return BadRequest("App User Login Failed. Reason: " + ex.Message);
                //return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("getUserDetailbyid")]
        public async Task<object> getUserDetailbyid(int user_id, int user_type)
        {
            try
            {
                if (user_type == 1)
                {
                    var user_details = _context.CustomerProfile.Where(x => x.user_id == user_id).Select(y => new { y.id, y.first_name, y.last_name,y.email }).FirstOrDefault();
                    var modl = new
                    {
                        statuscode = 200,
                        data = user_details,
                    };
                    return Ok(modl);
                }
                else
                {
                    var user_details = _context.ProviderProfile.Where(x => x.user_id == user_id).Select(y => new { y.id, y.first_name, y.last_name, y.email }).FirstOrDefault();
                    var modl = new
                    {
                        statuscode = 200,
                        data = user_details,
                    };
                    return Ok(modl);
                }

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, ex.Message);
                var modl = new
                {
                    statuscode = 200,
                    message = " not found",
                };
                return Unauthorized(modl);
                //return BadRequest("App User Login Failed. Reason: " + ex.Message);
                //return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [ActionName("UpdateUserProfile")]
        public async Task<object> UpdateUserProfile(ProfileModel _model)
        {


          try

            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                  
                     if (_model.user_type == 1)
                    {
                        var usermodel = _context.CustomerProfile.Where(x => x.user_id == _model.userid).FirstOrDefault();
                        usermodel.address = _model.address;
                        usermodel.first_name = _model.first_name;
                        usermodel.last_name = _model.last_name;
                        usermodel.suite_number = _model.suite;
                        usermodel.phone_1 = _model.phone_num1;
                        usermodel.phone_2 = _model.phone_num2;
                        usermodel.postal_code = _model.postal_code;
                        usermodel.state_id = _model.area;
                        usermodel.country_id = _model.country;
                        usermodel.city_id = _model.city;
                        usermodel.updated_by = _model.userid;
                        usermodel.updated_at = System.DateTime.Now;
                        _context.CustomerProfile.Update(usermodel);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var usermodel = _context.ProviderProfile.Where(x => x.user_id == _model.userid).FirstOrDefault();
                        usermodel.address = _model.address;
                        usermodel.first_name = _model.first_name;
                        usermodel.last_name = _model.last_name;
                        usermodel.suite_number = _model.suite;
                        usermodel.phone_1 = _model.phone_num1;
                        usermodel.phone_2 = _model.phone_num2;
                        usermodel.postal_code = _model.postal_code;
                        usermodel.state_id = _model.area;
                        usermodel.country_id = _model.country;
                        usermodel.city_id = _model.city;
                        usermodel.updated_by = _model.userid;
                        usermodel.updated_at = System.DateTime.Now;
                        usermodel.description = _model.description;
                        _context.ProviderProfile.Update(usermodel);
                        _context.SaveChanges();
                        }
                    }
                        /// add login Details///

                       

                    var modl = new
                    {
                        statuscode = 200,
                        message = "Update Successfully",
                        //data = getuser
                    };
                    return Ok(modl);



                }


                catch (Exception ex)
                {
                    //_logger.LogError(ex, ex.Message);
                    var modl = new
                    {
                        statuscode = 200,
                        message = "Update failed",
                    };
                    return Unauthorized(modl);
                    //return BadRequest("App User Login Failed. Reason: " + ex.Message);
                    //return BadRequest(ex.Message);
                }
            
            
        }

        [HttpPut]
        [ActionName("UpdateProfileImage")]
        public async Task<object> UpdateProfileImage(ImageModel _model)
        {
            //using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))// new TransactionScope())
            //{
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    else
                    {

                        if (_model.user_type == 1)
                        {
                            var usermodel = _context.CustomerProfile.Where(x => x.user_id == _model.user_id).FirstOrDefault();
                            usermodel.image_url = _model.image_url;
                            usermodel.updated_by = _model.user_id;
                            usermodel.updated_at = System.DateTime.Now;
                            _context.CustomerProfile.Update(usermodel);
                            _context.SaveChanges();
                            //transaction.Commit();
                        }
                        else
                        {
                            var usermodel = _context.ProviderProfile.Where(x => x.user_id == _model.user_id).FirstOrDefault();
                            usermodel.image_url = _model.image_url;
                            usermodel.updated_by = _model.user_id;
                            usermodel.updated_at = System.DateTime.Now;
                            _context.ProviderProfile.Update(usermodel);
                            _context.SaveChanges();
                           // transaction.Commit();
                        }
                    }
                    /// add login Details///



                    var modl = new
                    {
                        statuscode = 200,
                        message = "Update Successfully",
                        //data = getuser
                    };
                    return Ok(modl);



                }


                catch (Exception ex)
                {
                    //_logger.LogError(ex, ex.Message);
                    var modl = new
                    {
                        statuscode = 200,
                        message = "Update failed",
                    };
                    //transaction.Rollback();
                    //transaction.Dispose();
                    return Unauthorized(modl);
                    //return BadRequest("App User Login Failed. Reason: " + ex.Message);
                    //return BadRequest(ex.Message);
                }
           // }

        }

        [HttpGet]
        [ActionName("VerifyEmailExists")]
        public async Task<object> VerifyEmailExists(string email)
        {
            try
            {
                
                    var check_user =await _context.Users.Where(x => x.email == email).FirstOrDefaultAsync();
                if (check_user == null)
                {
                    var modl = new
                    {
                        statuscode = 200,
                        message = "not exist"
                    };
                    return Ok(modl);
                }
                else
                {
                    var modl = new
                    {
                        statuscode = 201,
                        message = "exist"
                    };
                    return Ok(modl);
                }
                    
                
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, ex.Message);
                var modl = new
                {
                    statuscode = 404,
                    message = "not found",
                };
                return Unauthorized(modl);
                //return BadRequest("App User Login Failed. Reason: " + ex.Message);
                //return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [ActionName("verifyUserName")]
        public async Task<object> verifyUserName(string username)
        {
            try
            {

                var check_user = await _context.Users.Where(x => x.username == username).FirstOrDefaultAsync();
                if (check_user == null)
                {
                    var modl = new
                    {
                        statuscode = 200,
                        message = "user not exist"
                    };
                    return Ok(modl);
                }
                else
                {
                    var modl = new
                    {
                        statuscode = 201,
                        message = "user exist"
                    };
                    return Ok(modl);
                }


            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, ex.Message);
                var modl = new
                {
                    statuscode = 404,
                    message = "not found",
                };
                return Unauthorized(modl);
                //return BadRequest("App User Login Failed. Reason: " + ex.Message);
                //return BadRequest(ex.Message);
            }
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


        [HttpGet]
        [ActionName("Encrypt")]
        public  string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
    }
}
