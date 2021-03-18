using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkilledWorkersService.Model;
using SkilledWorkersService.Models;
using UserManagementService.Models;

namespace SkilledWorkersService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SkilledServicesController : Controller
    {
        private readonly _dbContext _context;
        public SkilledServicesController(_dbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [ActionName("GetCategoriesList")]
        public async Task<object> GetCategoriesList()
        {
            try
            {
                var geCategoriesList =await  _context.Categories.Where(x => x.status ==true).Select(y => new { y.id, y.category_name }).ToListAsync();
                if (geCategoriesList.Count>0)
                {
                    var modl = new
                    {
                        statuscode = 200,
                        message = "Data Found",
                        data = geCategoriesList
                    };
                    return Ok(modl);
                   
                }
                else
                {
                    var modl = new
                    {
                        statuscode = 204,
                        message = "Data Not Found"
                    };
                    return Ok(modl);

                }
            }
            catch (Exception ex)
            {
                return BadRequest("Data Not Found. Reason: " + ex.Message);
            }
        }
        [HttpGet]
        [ActionName("GetSubCategoriesList")]
        public async Task<object> GetSubCategoriesList()
        {
            try
            {
                var geCategoriesList = await _context.SubCategories.Where(x => x.status == true).Select(y=>new { y.id,y.category_name,y.category_id}).ToListAsync();
                if (geCategoriesList.Count > 0)
                {
                    var modl = new
                    {
                        statuscode = 200,
                        message = "Data Found",
                        data = geCategoriesList
                    };
                    return Ok(modl);

                }
                else
                {
                    var modl = new
                    {
                        statuscode = 204,
                        message = "Data Not Found"
                    };
                    return Ok(modl);

                }
            }
            catch (Exception ex)
            {
                return BadRequest("Data Not Found. Reason: " + ex.Message);
            }
        }
        [HttpGet]
        [ActionName("GetCountriesList")]
        public async Task<object> GetCountriesList()
        {
            try
            {
                var geCountriesList = await _context.Countries.Where(x => x.status == true).Select(y => new { y.id, y.title }).ToListAsync();
                if (geCountriesList.Count > 0)
                {
                    var modl = new
                    {
                        statuscode = 200,
                        message = "Data Found",
                        data = geCountriesList
                    };
                    return Ok(modl);
                }
                else
                {
                    var modl = new
                    {
                        statuscode = 204,
                        message = "Data Not Found"
                    };
                    return Ok(modl);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Data Not Found. Reason: " + ex.Message);
            }
        }
        [HttpGet]
        [ActionName("GetCitiesList")]
        public async Task<object> GetCitiesList()
        {
            try
            {
                var geCitiesList = await _context.Cities.Where(x => x.status == true).Select(y => new { y.id, y.title,y.country_id }).ToListAsync();
                if (geCitiesList.Count > 0)
                {
                    var modl = new
                    {
                        statuscode = 200,
                        message = "Data Found",
                        data = geCitiesList
                    };
                    return Ok(modl);
                }
                else
                {
                    var modl = new
                    {
                        statuscode = 204,
                        message = "Data Not Found"
                    };
                    return Ok(modl);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Data Not Found. Reason: " + ex.Message);
            }
        }

        [HttpGet]
        [ActionName("GetCurrenciesList")]
        public async Task<object> GetCurrenciesList()
        {
            try
            {
                var geCurrenciesList = await _context.currencies.Where(x => x.status == true).Select(y => new { y.id, y.title, y.short_code }).ToListAsync();
                if (geCurrenciesList.Count > 0)
                {
                    var modl = new
                    {
                        statuscode = 200,
                        message = "Data Found",
                        data = geCurrenciesList
                    };
                    return Ok(modl);
                }
                else
                {
                    var modl = new
                    {
                        statuscode = 204,
                        message = "Data Not Found"
                    };
                    return Ok(modl);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Data Not Found. Reason: " + ex.Message);
            }
        }
        [HttpGet]
        [ActionName("GetAreaList")]
        public async Task<object> GetAreaList()
        {
            try
            {
                var geAreaList = await _context.Area.Where(x => x.status == true).Select(y => new { y.id, y.title, y.city_id }).ToListAsync();
                if (geAreaList.Count > 0)
                {
                    var modl = new
                    {
                        statuscode = 200,
                        message = "Data Found",
                        data = geAreaList
                    };
                    return Ok(modl);
                }
                else
                {
                    var modl = new
                    {
                        statuscode = 204,
                        message = "Data Not Found"
                    };
                    return Ok(modl);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Data Not Found. Reason: " + ex.Message);
            }
        }

        [HttpGet]
        [ActionName("findProviderServices")]
        public async Task<object> findProviderServices(string keyword, string address, int? category,int? sub_category, int? city, int? area )
        {
            try
            {

               //S var user_details = _context.ProviderProfile.Where(x => x.city_id == Convert.ToInt32(city)).FirstOrDefaultAsync();
                var providersList =await (from pp in _context.ProviderProfile
                                          join cat in _context.Categories on pp.category_id equals cat.id
                                          join sub_cat in _context.SubCategories on pp.sub_category_id equals sub_cat.id
                                          join pc in _context.Cities on pp.city_id equals pc.id
                                          join pct in _context.Countries on pp.country_id equals pct.id
                                          where (pp.category_id == category || (address.Contains(pp.address) || pp.city_id==city || pp.state_id==area ))
                           select new
                           {
                               pp.id,
                               pp.image_url,
                               pp.first_name,
                               pp.last_name,
                               pp.address,
                               category = cat.category_name,
                               sub_category = sub_cat.category_name,
                               city = pc.title,
                               country = pct.title,

                           }).ToListAsync();
                var modl = new
                    {
                        statuscode = 200,
                    // data = user_details,
                       providersList = providersList,
                    };
                    return Ok(modl);
                

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
        [ActionName("findCategorybyParams")]
        public async Task<object> findCategorybyParams(string category)
        {
            try
            {
                List<object> _getCatAndProviders = new List<object>();
                var category_id =await _context.Categories.Where(x => x.category_name == category).Select(x=>x.id).FirstOrDefaultAsync();
                if(category_id != 0)
                {
                    var getSubCatList = await (from sub in _context.SubCategories
                                               where sub.category_id == category_id
                                               select new
                                               {
                                                   sub.id,
                                                   sub.category_id,
                                                   sub.category_name,
                                                   sub.status,
                                                   sub.category_icon,
                                                   provider_count=_context.ProviderProfile.Where(x=>x.category_id== sub.id).Count()
                                               }).ToListAsync();
                        
                        //_context.SubCategories.Where(x => x.category_id == category_id).ToListAsync();
                 
                   _getCatAndProviders.Add(getSubCatList);

                    var providersList = await (from pp in _context.ProviderProfile
                                               join cat in _context.Categories on pp.category_id equals cat.id
                                               join sub_cat in _context.SubCategories on pp.sub_category_id equals sub_cat.id
                                               join pc in _context.Cities on pp.city_id equals pc.id
                                               join pct in _context.Countries on pp.country_id equals pct.id
                                               where (pp.category_id == category_id)
                                               select new
                                               {
                                                   pp.id,
                                                   pp.image_url,
                                                   pp.first_name,
                                                   pp.last_name,
                                                   pp.address,
                                                   category = cat.category_name,
                                                   sub_category = sub_cat.category_name,
                                                   city = pc.title,
                                                   country = pct.title,
                                                   cat_icon = cat.category_icon,
                                               }).ToListAsync();

                    _getCatAndProviders.Add(providersList);
                }
               

                var modl = new
                {
                    statuscode = 200,
                    // data = user_details,
                    result = _getCatAndProviders,
                };
                return Ok(modl);


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
        [ActionName("findProviderProfileByID")]
        public async Task<object> findProviderProfileByID(int? provider_id)
        {
            try
            {
               
                    var providersList = await (from pp in _context.ProviderProfile
                                               join cat in _context.Categories on pp.category_id equals cat.id
                                               join sub_cat in _context.SubCategories on pp.sub_category_id equals sub_cat.id
                                               join pc in _context.Cities on pp.city_id equals pc.id
                                               join pct in _context.Countries on pp.country_id equals pct.id
                                               where (pp.id == provider_id)
                                               select new
                                               {
                                                   pp.id,
                                                   pp.image_url,
                                                   pp.first_name,
                                                   pp.last_name,
                                                   pp.address,
                                                   category = cat.category_name,
                                                   sub_category = sub_cat.category_name,
                                                   sub_cat_id= sub_cat.id,
                                                   city = pc.title,
                                                   country = pct.title,
                                                   cat_icon = cat.category_icon,
                                                   description = pp.description
                                               }).ToListAsync();


                var servicesList= await (from ss in _context.UserServices
                                        
                                         where (ss.provider_id == providersList[0].id)
                                         select new
                                         {
                                             ss.id,
                                             ss.per_hour_rate,
                                             ss.provider_id,
                                             ss.service_detail,
                                             ss.service_name,
                                         }).ToListAsync();

                var modl = new
                {
                    statuscode = 200,
                    // data = user_details,
                    result = providersList,
                    servicesList= servicesList
                };
               return Ok(modl);


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
        [ActionName("getProvidersbySubCatID")]
        public async Task<object> getProvidersbySubCatID(int? sub_cat_id)
        {
            try
            {

                var providersList = await (from pp in _context.ProviderProfile
                                           join cat in _context.Categories on pp.category_id equals cat.id
                                           join sub_cat in _context.SubCategories on pp.sub_category_id equals sub_cat.id
                                           join pc in _context.Cities on pp.city_id equals pc.id
                                           join pct in _context.Countries on pp.country_id equals pct.id
                                           where (pp.sub_category_id == sub_cat_id)
                                           select new
                                           {
                                               pp.id,
                                               pp.image_url,
                                               pp.first_name,
                                               pp.last_name,
                                               pp.address,
                                               category = cat.category_name,
                                               sub_category = sub_cat.category_name,
                                               city = pc.title,
                                               country = pct.title,
                                               cat_icon = cat.category_icon,
                                               description= pp.description
                                           }).ToListAsync();


                var modl = new
                {
                    statuscode = 200,
                    // data = user_details,
                    result = providersList,
                };
                return Ok(modl);


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

        public async Task<object> BookService(booking_model _model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {

                    ServiceBooking model = new ServiceBooking();
                    model.customer_suiteNumber = _model.apt_suite;
                    model.task_description = _model.shortdesc;
                    model.user_id = _model.user_id;
                    model.booking_date = Convert.ToDateTime(_model.bookingDate);
                    model.booking_sub_category_id = _model.booking_sub_category_id;
                    model.created_date = System.DateTime.Now;
                    model.customer_address = _model.customer_address;
                    model.customer_area_id = Convert.ToInt32(_model.selected_area);
                    model.customer_firstName = _model.firstName;
                    model.customer_LastName = _model.lastName;
                    model.customer_phone1 = _model.Phone1;
                    model.customer_phone2 = _model.phone2;
                    model.customer_Email = _model.email;
                    model.customer_address = _model.customer_address;
                    model.customer_city_id = Convert.ToInt32(_model.selected_city);
                    model.customer_country_id = Convert.ToInt32(_model.selected_country);
                    model.customer_address = _model.customer_address;
                    model.provider_id = _model.providerID;
                    model.booking_sub_category_id = _model.booking_sub_category_id;

                    _context.ServiceBooking.Add(model);
                   await _context.SaveChangesAsync();

                    Payment_Details p_model = new Payment_Details();
                    p_model.booking_id = model.id;
                    p_model.payer_name = _model.payer_name;
                    p_model.payer_email = _model.payer_email;
                    p_model.payer_address = _model.payer_address;
                    p_model.payer_id = _model.payer_id;
                    p_model.user_id = _model.user_id;
                    p_model.paymet_amount = _model.transction_amount;
                    p_model.currency_code = _model.payment_currency_code;
                    p_model.payment_status = true;
                    _context.Payment_Details.Add(p_model);
                    await _context.SaveChangesAsync();

                    var modl = new
                    {
                        statuscode = 200,
                        message = "Added Successfully",

                    };
                    return Ok(modl);



                }

            }
            catch (Exception ex)
            {
                var modl = new
                {
                    statuscode = 201,
                    message = "update failed",

                };
                return Ok(modl);
            }
        }

        public async Task<object> BookingRequests(int provider_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {

                    var get_booking = await (from booking in _context.ServiceBooking
                                      join sub_cat in _context.SubCategories on booking.booking_sub_category_id equals sub_cat.id
                                      join payemt_detail in _context.Payment_Details on booking.id equals payemt_detail.booking_id

                                      where (booking.provider_id == _context.ProviderProfile.Where(x=>x.user_id== provider_id).Select(x=>x.id).FirstOrDefault())
                                      select new
                                      {
                                          id = booking.id,
                                          customer_name = booking.customer_firstName + " " + booking.customer_LastName,
                                          customer_contact = booking.customer_phone1,
                                          category = sub_cat.category_name,
                                          booking_date = booking.booking_date,
                                          service_amount = payemt_detail.paymet_amount,
                                          payment_status = payemt_detail.payment_status 
                                      }).ToListAsync();
                    

                    var modl = new
                    {
                        statuscode = 200,
                        result = get_booking,

                    };
                    return Ok(modl);



                }

            }
            catch (Exception ex)
            {
                var modl = new
                {
                    statuscode = 201,
                    message = "update failed",

                };
                return Ok(modl);
            }
        }


        public async Task<object> BookingHistory(int user_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {

                    var get_booking = await (from booking in _context.ServiceBooking
                                             join sub_cat in _context.SubCategories on booking.booking_sub_category_id equals sub_cat.id
                                             join payemt_detail in _context.Payment_Details on booking.id equals payemt_detail.booking_id

                                             where (booking.user_id == user_id)
                                             select new
                                             {
                                                 id = booking.id,
                                                 customer_name = booking.customer_firstName + " " + booking.customer_LastName,
                                                 customer_contact = booking.customer_phone1,
                                                 category = sub_cat.category_name,
                                                 booking_date = booking.booking_date,
                                                 service_amount = payemt_detail.paymet_amount,
                                                 payment_status = payemt_detail.payment_status
                                             }).ToListAsync();


                    var modl = new
                    {
                        statuscode = 200,
                        result = get_booking,

                    };
                    return Ok(modl);



                }

            }
            catch (Exception ex)
            {
                var modl = new
                {
                    statuscode = 201,
                    message = "Data Not Found",

                };
                return Ok(modl);
            }
        }


        public async Task<object> getJobDetailbyid(int job_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {

                    var get_booking = await (from job in _context.UserGigs
                                             join job_type in _context.GigsType on job.gig_type equals job_type.id
                                             join user_detail in _context.Users on Convert.ToInt32(job.created_by) equals user_detail.id

                                             where (job.id == job_id)
                                             select new
                                             {
                                                 id = job.id,
                                                 title = job.user_gig_name ,
                                                 job_type = job_type.title,
                                                 job_location = job.gig_location,
                                                 job_posted_date = job.created_date,
                                                 post_by = user_detail.username,
                                                 job_hours = job.gig_hours,
                                                 job_cost = job.gig_rates,
                                                 job_description= job.gig_description,
                                                 
                                             }).ToListAsync();


                    var modl = new
                    {
                        statuscode = 200,
                        result = get_booking,

                    };
                    return Ok(modl);



                }

            }
            catch (Exception ex)
            {
                var modl = new
                {
                    statuscode = 201,
                    message = "update failed",

                };
                return Ok(modl);
            }
        }

        public async Task<object> getPremiumUsersList()
        {
            try
            {
                    List<object> ServicesList = new List<object>();
                    var get_booking = await (from pp in _context.ProviderProfile
                                             join city in _context.Cities on pp.city_id equals city.id
                                             join country in _context.Countries on pp.country_id equals country.id

                                             where (pp.premium_user == true)
                                             select new
                                             {
                                                 id=pp.id,
                                                 user_id = pp.user_id,
                                                 image= pp.image_url,
                                                 first_name=pp.first_name,
                                                 last_name= pp.last_name,
                                                 description= pp.description,
                                                 city=city.title,
                                                 country= country.title

                                             }).ToListAsync();

                if (get_booking.Count() > 0)
                {
                    foreach(var item in get_booking)
                    {
                        var getProviderServices = _context.UserServices.Where(x => x.provider_id == item.id).ToList();
                        if (getProviderServices.Count() > 0)
                        {
                            ServicesList.Add(getProviderServices);
                        }
                    }
                }
                        
                  

                    var modl = new
                    {
                        statuscode = 200,
                        result = get_booking,
                        p_services= ServicesList

                    };
                    return Ok(modl);


            }
            catch (Exception ex)
            {
                var modl = new
                {
                    statuscode = 201,
                    message = "not found",

                };
                return Ok(modl);
            }
        }


        [HttpPut]
        [ActionName("PostService")]
        public async Task<object> PostService(ServiceModel _model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    UserServices serviceModel = new UserServices();
                    if (_model.id == 0)
                    {
                        serviceModel.id = 0;
                        serviceModel.service_name = _model.service_name;
                        serviceModel.service_detail = _model.service_details;
                        serviceModel.per_hour_rate = _model.service_cost.ToString();
                        serviceModel.status = true;
                        serviceModel.provider_id = _context.ProviderProfile.Where(x => x.user_id == _model.user_id).Select(x => x.id).FirstOrDefault();
                        _context.Add(serviceModel);
                        _context.SaveChanges();
                        var modl = new
                        {
                            statuscode = 200,
                            message = "Added Successfully",

                        };
                        return Ok(modl);

                    }
                    else
                    {
                        serviceModel.id = _model.id;
                        serviceModel.service_name = _model.service_name;
                        serviceModel.service_detail = _model.service_details;
                        serviceModel.per_hour_rate = _model.service_cost.ToString();
                        serviceModel.provider_id = _context.ProviderProfile.Where(x => x.user_id == _model.user_id).Select(x => x.id).FirstOrDefault();
                        serviceModel.status = _model.status == "true" ? true : false;
                        

                        _context.Update(serviceModel);
                        _context.SaveChanges();
                        var modl = new
                        {
                            statuscode = 200,
                            message = "Updated Successfully",

                        };
                        return Ok(modl);
                    }
                    
                    



                }

            }
            catch (Exception ex)
            {
                var modl = new
                {
                    statuscode = 201,
                    message = "update failed",

                };
                return Ok(modl);
            }
        }


        public async Task<object> getServicesbyProviderID(string user_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {

                    var get_booking = await (from service in _context.UserServices
                                             //join sub_cat in _context.SubCategories on booking.booking_sub_category_id equals sub_cat.id
                                             //join payemt_detail in _context.Payment_Details on booking.id equals payemt_detail.booking_id

                                             where (service.provider_id == _context.ProviderProfile.Where(x=>x.user_id== Convert.ToInt32(user_id) ).Select(x=>x.id).FirstOrDefault())
                                             select new
                                             {
                                                 id = service.id,
                                                 service_name = service.service_name,
                                                 service_description = service.service_detail,
                                                 status = service.status== true? "Active":"Inactive",
                                                 rate=service.per_hour_rate,
                                             }).ToListAsync();


                    var modl = new
                    {
                        statuscode = 200,
                        result = get_booking,

                    };
                    return Ok(modl);



                }

            }
            catch (Exception ex)
            {
                var modl = new
                {
                    statuscode = 201,
                    message = "update failed",

                };
                return Ok(modl);
            }
        }

    }
}