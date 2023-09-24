using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVP.Date;
using MVP.Date.Api;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVP.Controllers
{
    public class ApiController : ControllerBase
    {
        private readonly AppDB _appDB;
        private readonly ITask _task;
        private readonly IMaterial _material;
        private readonly IProject _proj;
        private readonly IUser _user;

        public ApiController(ITask task, IMaterial materia, IProject proj, IUser user, AppDB appDb)
        {
            _appDB = appDb;
            _task = task;
            _material = materia;
            _proj = proj;
            _user = user;
        }
        //add generate token
        #region ////////////////// User functions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userParam"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegUser([FromBody] UserParam userParam)
        {
            var name = userParam.name.Trim();
            var seurName = userParam.seurname.Trim();
            var mail = userParam.mail.Trim();
            var post = userParam.post.Trim();
            var pwd = userParam.passvord.Trim();

            User user = null;
            try
            {
                user = _user.GetAllUsers.FirstOrDefault(p => p.mail == mail);
            }
            catch (Exception) { }

            if (user != null)
            {
                return new JsonResult(new ObjectResult(new { message = "Пользователь с этой почтой уже существует!" }) { StatusCode = 400 }); 
            }

            if (name == null || seurName == null || mail == null || post == null || pwd == null)
            {
                return new JsonResult(new ObjectResult(new { message = "Поля не могут быть пустыми!" }) { StatusCode = 400 });
            }

            var newUser = new User
            {
                name = name,
                seurname = seurName,
                mail = mail,
                post = post,
                passvord = pwd,
                token = null//refresh
            };
            _user.AddToDb(user);

            var outt = new
            {
                message = "Пользователь успешно зарегистрирован!",
                tasks = newUser
            };

            return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userParam"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TryLogin([FromBody] UserParam userParam)
        {
            var login = userParam.mail.Trim();
            var pwd = userParam.passvord.Trim();

            User user = null;
            try
            {
                user = _user.GetAllUsers.FirstOrDefault(p => p.mail == login);
            }
            catch (Exception) { }

            if (user != null)
            {
                if(user.passvord != pwd)
                {
                    return new JsonResult(new ObjectResult(new { message = "Неверный пароль!" }) { StatusCode = 400 });
                }
                else
                {
                    user.token = null;//refresh
                    var outt = new
                    {
                        message = "Успешный вход!",
                        tasks = user
                    };

                    return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });
                }
            }
            else
            {
                return new JsonResult(new ObjectResult(new { message = "Пользователь не зарегистрирован!!" }) { StatusCode = 400 });
            }

        }
        #endregion


        #region ////////////////// Task functions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskParam"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskParam"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddTask([FromBody] TaskParam taskParam)
        {
            User user = null;
            try
            {
                user = _appDB.DBUser.FirstOrDefault(p => p.token == taskParam.token);
                if (user == null || user == new User())
                {
                    return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 });
                }
            }
            catch (Exception) { return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 }); }

            // convert Date
            var planStartDate = new DateTime(1, 1, 1);
            var factStartDate = new DateTime(1, 1, 1);

            var planFinishDate = new DateTime(1, 1, 1);
            var factFinishDate = new DateTime(1, 1, 1);

            var planPayDate = new DateTime(1, 1, 1);
            var factPayDate = new DateTime(1, 1, 1);

            try
            {
                planStartDate = DateTime.Parse(taskParam.planStartDate);
            }
            catch (Exception) { }

            
            var name = taskParam.name.Trim();
            var prijId = taskParam.prijId;
            var supervisor = taskParam.supervisor.Trim();
            var planedPrice = taskParam.planedPrice;
            var factPrice = taskParam.factPrice;
            var parentTaskName = taskParam.parentTaskName?.Trim();
            var parentTaskId = taskParam.parentTaskId;
            var materials = taskParam.materials.Trim();
            var necesseMaterials = taskParam.necesseMaterials.Trim();
            var planedMaterialPrice = taskParam.planMaterialPrice;
            var factMaterialPrice = taskParam.factMaterialPrice;

            if (name == null || prijId == 0 || supervisor == null || planedPrice == 0 || materials == null
                || planStartDate == null || planFinishDate == null || planPayDate == null )
            {
                return new JsonResult(new ObjectResult(new { message = "Поля не могут быть пустыми!" }) { StatusCode = 400 });
            }

            var task = new Task
            {
                name = name,
                prijId = prijId,
                supervisor = supervisor,
                planStartDate = planStartDate,
                factStartDate = factStartDate,
                planFinishDate = planFinishDate,
                factFinishDate = factFinishDate,
                planPayDate = planPayDate,
                factPayDate = factPayDate,
                planedPrice = planedPrice,
                factPrice = factPrice,
                parentTaskName = parentTaskName,
                parentTaskId = parentTaskId,
                materials = materials,
                necesseMaterials = necesseMaterials,
                planedMaterialPrice = planedMaterialPrice,
                factMaterialPrice = factMaterialPrice
            };
            _task.AddToDb(task);

            var outt = new
            {
                mesage = "Задача/работа успешно добавлена!",
                tasks = task
            };

            return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskParam"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RedactTask([FromBody] TaskParam taskParam)
        {
            User user = null;
            try
            {
                user = _appDB.DBUser.FirstOrDefault(p => p.token == taskParam.token);
                if (user == null || user == new User())
                {
                    return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 });
                }
            }
            catch (Exception) { return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 }); }

            Task result = null;
            try
            {
                result = _appDB.DBTask.FirstOrDefault(p => p.id == taskParam.id);
                if (result == null || result == new Task())
                {
                    return new JsonResult(new ObjectResult(new { message = "Указанная задача не найдена!" }) { StatusCode = 400 });
                }
            }
            catch (Exception) { return new JsonResult(new ObjectResult(new { message = "Указанная задача не найдена!" }) { StatusCode = 400 }); }

            var planStartDate = new DateTime(1, 1, 1);
            var factStartDate = new DateTime(1, 1, 1);

            var planFinishDate = new DateTime(1, 1, 1);
            var factFinishDate = new DateTime(1, 1, 1);

            var planPayDate = new DateTime(1, 1, 1);
            var factPayDate = new DateTime(1, 1, 1);

            try
            {
                planStartDate = DateTime.Parse(taskParam.planStartDate);
            }
            catch (Exception) { }

            if (!_task.RedactToDb(taskParam.id, taskParam.name,planStartDate, factStartDate, planFinishDate, factFinishDate, planPayDate, factPayDate, taskParam.planedPrice, taskParam.factPrice, taskParam.parentTaskName, taskParam.materials, taskParam.necesseMaterials, taskParam.supervisor, taskParam.planMaterialPrice, taskParam.factMaterialPrice))
            {
                return new JsonResult(new ObjectResult(new { message = "Ошибка редактирования!" }) { StatusCode = 400 });
            }
            else
            {
                var task = _task.GetTask(taskParam.id);
                var outt = new
                {
                    mesage = "Задача/работа успешно обновлена!",
                    tasks = task
                };

                return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });

            }

        }
        #endregion


        #region ////////////////// Project functions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projParam"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projParam"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddProj([FromBody] ProjParam projParam)
        {
            User user = null;
            try
            {
                user = _appDB.DBUser.FirstOrDefault(p => p.token == projParam.token);
                if (user == null || user == new User())
                {
                    return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 });
                }
            }
            catch (Exception) { return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 }); }

            // convert Date
            var planStartDate = new DateTime(1, 1, 1);
            var factStartDate = new DateTime(1, 1, 1);

            var planFinishDate = new DateTime(1, 1, 1);
            var factFinishDate = new DateTime(1, 1, 1);


            try
            {
                planStartDate = DateTime.Parse(projParam.planStartDate);
            }
            catch (Exception) { }


            var name = projParam.name.Trim();
            var adr = projParam.adr.Trim();
            var supervisor = projParam.supervisor.Trim();

            var planWorkPrice = projParam.planWorkPrice;
            var factWorkPrice = projParam.factWorkPrice;

            var planMaterialPrice = projParam.planMaterialPrice;
            var factMaterialPrice = projParam.factMaterialPrice;

            if (name == null || adr == null || supervisor == null || planWorkPrice == 0 || planMaterialPrice == 0
                || planStartDate == null || planFinishDate == null)
            {
                return new JsonResult(new ObjectResult(new { message = "Поля не могут быть пустыми!" }) { StatusCode = 400 });
            }

            var proj = new Project
            {
                name = name,
                adr = adr,
                supervisor = supervisor,
                planStartDate = planStartDate,
                factStartDate = factStartDate,
                planFinishDate = planFinishDate,
                factFinishDate = factFinishDate,
                planWorkPrice = planWorkPrice,
                factWorkPrice = factWorkPrice,
                planMaterialPrice = planMaterialPrice,
                factMaterialPrice = factMaterialPrice
            };
            _proj.AddToDb(proj);

            var outt = new
            {
                mesage = "Проект успешно добавлен!",
                tasks = proj
            };

            return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projParam"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RedacProj([FromBody] ProjParam projParam)
        {
            User user = null;
            try
            {
                user = _appDB.DBUser.FirstOrDefault(p => p.token == projParam.token);
                if (user == null || user == new User())
                {
                    return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 });
                }
            }
            catch (Exception) { return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 }); }

            Task result = null;
            try
            {
                result = _appDB.DBTask.FirstOrDefault(p => p.id == projParam.id);
                if (result == null || result == new Task())
                {
                    return new JsonResult(new ObjectResult(new { message = "Указанный проект не найден!" }) { StatusCode = 400 });
                }
            }
            catch (Exception) { return new JsonResult(new ObjectResult(new { message = "Указанный проект не найден!" }) { StatusCode = 400 }); }

            var planStartDate = new DateTime(1, 1, 1);
            var factStartDate = new DateTime(1, 1, 1);

            var planFinishDate = new DateTime(1, 1, 1);
            var factFinishDate = new DateTime(1, 1, 1);


            try
            {
                planStartDate = DateTime.Parse(projParam.planStartDate);
            }
            catch (Exception) { }


            var name = projParam.name.Trim();
            var adr = projParam.adr.Trim();
            var supervisor = projParam.supervisor.Trim();

            var planWorkPrice = projParam.planWorkPrice;
            var factWorkPrice = projParam.factWorkPrice;

            var planMaterialPrice = projParam.planMaterialPrice;
            var factMaterialPrice = projParam.factMaterialPrice;

            if (!_proj.RedactToDb(projParam.id,
                planStartDate,
                factStartDate,
                planFinishDate,
                factFinishDate,
                projParam.name,
                projParam.adr,
                projParam.supervisor,
                projParam.planWorkPrice,
                projParam.factWorkPrice,
                projParam.planMaterialPrice,
                projParam.factMaterialPrice
            ))
            {
                return new JsonResult(new ObjectResult(new { message = "Ошибка редактирования!" }) { StatusCode = 400 });
            }
            else
            {
                var task = _proj.GetProj(projParam.id);

                var outt = new
                {
                    mesage = "Проект успешно обновлен!",
                    tasks = task
                };

                return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });

            }

        }
        #endregion


        #region ////////////////// Material function
        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialParam"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialParam"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddMaterial([FromBody] MaterialParam materialParam)
        {
            User user = null;
            try
            {
                user = _appDB.DBUser.FirstOrDefault(p => p.token == materialParam.token);
                if (user == null || user == new User())
                {
                    return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 });
                }
            }
            catch (Exception) { return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 }); }

            // convert Date
            var planPayDate = new DateTime(1, 1, 1);
            var factPayDate = new DateTime(1, 1, 1);

            try
            {
                planPayDate = DateTime.Parse(materialParam.planPayDate);
            }
            catch (Exception) { }


            var name = materialParam.name.Trim();
            var category = materialParam.category.Trim();

            var countName = materialParam.countName.Trim();
            var planCount = materialParam.planCount;
            var factCount = materialParam.factCount;
            var planPrice = materialParam.planPrice;
            var factPrice = materialParam.factPrice;

            var status = materialParam.status.Trim();

            var taskId = materialParam.taskId;
            var taskName = materialParam.taskName.Trim();

            var projId = materialParam.projId;
            var provider = materialParam.provider.Trim();

            if (name == null || countName == null || planCount == 0 || planPrice == 0 || status == null
                || provider == null)
            {
                return new JsonResult(new ObjectResult(new { message = "Поля не могут быть пустыми!" }) { StatusCode = 400 });
            }

            var material = new Material
            {
                name = name,
                category = category,
                countName = countName,
                planCount = planCount,
                factCount = factCount,
                planPrice = planPrice,
                factPrice = factPrice,
                status = status,
                planPayDate = planPayDate,
                factPayDate = factPayDate,
                taskId = taskId,
                taskName = taskName,
                projId = projId,
                provider = provider
            };
            _material.AddToDb(material);

            var outt = new
            {
                mesage = "Материал успешно добавлен!",
                tasks = material
            };

            return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialParam"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RedacMaterial([FromBody] MaterialParam materialParam)
        {
            User user = null;
            try
            {
                user = _appDB.DBUser.FirstOrDefault(p => p.token == materialParam.token);
                if (user == null || user == new User())
                {
                    return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 });
                }
            }
            catch (Exception) { return new JsonResult(new ObjectResult(new { message = "Нет полномочий!" }) { StatusCode = 400 }); }

            Task result = null;
            try
            {
                result = _appDB.DBTask.FirstOrDefault(p => p.id == materialParam.id);
                if (result == null || result == new Task())
                {
                    return new JsonResult(new ObjectResult(new { message = "Указанный материал не найден!" }) { StatusCode = 400 });
                }
            }
            catch (Exception) { return new JsonResult(new ObjectResult(new { message = "Указанный материал не найден!" }) { StatusCode = 400 }); }

            var planPayDate = new DateTime(1, 1, 1);
            var factPayDate = new DateTime(1, 1, 1);

            try
            {
                planPayDate = DateTime.Parse(materialParam.planPayDate);
            }
            catch (Exception) { }


            var name = materialParam.name.Trim();
            var category = materialParam.category.Trim();

            var countName = materialParam.countName.Trim();
            var planCount = materialParam.planCount;
            var factCount = materialParam.factCount;
            var planPrice = materialParam.planPrice;
            var factPrice = materialParam.factPrice;

            var status = materialParam.status.Trim();

            var taskId = materialParam.taskId;
            var taskName = materialParam.taskName.Trim();

            var projId = materialParam.projId;
            var provider = materialParam.provider.Trim();

            if (!_material.RedactToDb(materialParam.id,
                name,
                category,
                countName,
                planCount,
                factCount,
                planPrice,
                factPrice,
                status,
                taskName,
                provider,
                planPayDate,
                factPayDate
            ))
            {
                return new JsonResult(new ObjectResult(new { message = "Ошибка редактирования!" }) { StatusCode = 400 });
            }
            else
            {
                var material = _material.GetMaterial(materialParam.id);

                var outt = new
                {
                    mesage = "Материал успешно обновлен!",
                    tasks = material
                };

                return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });

            }

        }
        #endregion
    }
}
