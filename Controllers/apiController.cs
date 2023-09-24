using Microsoft.AspNetCore.Mvc;
using MVP.Date.Api;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVP.Controllers
{
    public class ApiController : ControllerBase
    {
        private readonly ITask _task;
        private readonly IMaterial _material;
        private readonly IProject _proj;

        public ApiController(ITask task, IMaterial materia, IProject proj)
        {
            _task = task;
            _material = materia;
            _proj = proj;
        }


        [HttpPost]
        public JsonResult GetTasks([FromBody] TaskParam taskParam)
        {
            List<Task> array = _task.GetAllTasks;

            try
            {
                
            }
            catch(Exception) { return new JsonResult(new ObjectResult(new {message = "Нет совпадений"}) { StatusCode = 400 }); }

            var outt = new
            {
                tasks = array
            };

            return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });

        }

        [HttpPost]
        public JsonResult GetProj([FromBody] ProjParam projParam)
        {
            List<Project> array = _proj.GetAllProjects;

            try
            {

            }
            catch (Exception) { return new JsonResult(new ObjectResult(new { message = "Нет совпадений" }) { StatusCode = 400 }); }

            var outt = new
            {
                tasks = array
            };

            return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });

        }

        [HttpPost]
        public JsonResult GetMaterial([FromBody] MaterialParam materialParam)
        {
            List<Material> array = _material.GetAllMaterials;

            try
            {

            }
            catch (Exception) { return new JsonResult(new ObjectResult(new { message = "Нет совпадений" }) { StatusCode = 400 }); }

            var outt = new
            {
                tasks = array
            };

            return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });

        }

       
    }
}
