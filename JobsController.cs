using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkilledWorkersService.Model;
using SkilledWorkersService.Models;

namespace SkilledWorkersService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JobsController : ControllerBase
    {

        private readonly _dbContext _context;
        public JobsController(_dbContext context)
        {
            _context = context;
        }
        public async Task<object> PostJob(PostJobModel _user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    UserGigs jobModel = new UserGigs();
                    jobModel.user_gig_name = _user.jobtitle;
                    jobModel.application_url = _user.applicationEmailUrl;
                    jobModel.category_id = _user.categoryName;
                    jobModel.sub_category_id = _user.SubcategoryName;
                    jobModel.gig_type = _user.jobtype;
                    jobModel.gig_location = _user.job_location;
                    jobModel.companyName = _user.companyName;
                    jobModel.gig_rates = _user.jobcost;
                    jobModel.gig_hours = _user.jobhours;
                    jobModel.gig_description = _user.jobdescription;
                    jobModel.created_date = System.DateTime.Now;
                    jobModel.created_by = _user.userid.ToString();
                    jobModel.status = true;
                    jobModel.currency_id = _user.CurrencyName;
                    _context.UserGigs.Add(jobModel);
                    _context.SaveChanges();
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

        [HttpGet]
        [ActionName("GetJobsList")]
        public async Task<object> GetJobsList()
        {
            try
            {

                var current_date = System.DateTime.Now;
                var job_list = await (from job in _context.UserGigs
                                           join type in _context.GigsType on job.gig_type equals type.id
                                           
                                           where (job.status == true)
                                           select new
                                           {
                                              id= job.id,
                                               title = job.user_gig_name,
                                               description = job.gig_description,
                                               location = job.gig_location,
                                               rate = job.gig_rates,
                                               job_type = job.gig_type,
                                               months_ago = ((current_date.Year - job.created_date.Year) * 12) + current_date.Month - job.created_date.Month,
                                               job_type_title=type.title,
                                               title_css_class=type.title_css_class
                                           }).ToListAsync();
                //var geAreaList = await _context.UserGigs.Where(x => x.status == true).Select
                //    (y => new 
                    
                //    { 
                //        y.id,
                //        title=y.user_gig_name,
                //        description=y.gig_description,
                //        location=y.gig_location,
                //        rate=y.gig_rates,
                //        job_type=y.gig_type,
                //        months_ago= ((current_date.Year - y.created_date.Year) * 12) + current_date.Month - y.created_date.Month
                //    }).ToListAsync();
                if (job_list.Count > 0)
                {
                    var modl = new
                    {
                        statuscode = 200,
                        message = "Data Found",
                        data = job_list
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
        [ActionName("jobTypesList")]
        public async Task<object> jobTypesList()
        {
            try
            {

                var current_date = System.DateTime.Now;
                var getTypeList = await _context.GigsType.Where(x=>x.status==true).ToListAsync();
                if (getTypeList.Count > 0)
                {
                    var modl = new
                    {
                        statuscode = 200,
                        message = "Data Found",
                        data = getTypeList
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

    }
}