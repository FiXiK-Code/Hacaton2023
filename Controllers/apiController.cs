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
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Task = MVP.Date.Models.Task;

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

        [HttpPost]
        public JsonResult Test([FromBody] UserParam userParam)
        {
            var outt = new
            {
                message = "Пользователь успешно зарегистрирован!",
                tasks = userParam
            };

            return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });
        }

        #region ////////////////// User functions
        /// <summary>
        /// Метод для регистрация пользователя в системе
        /// </summary>
        /// <param name="userParam">Обект предаствляющий входные данные</param>
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

            string refrash_tpken = null;
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refrash_tpken = Convert.ToBase64String(randomNumber);
            }

            var newUser = new User
            {
                name = name,
                seurname = seurName,
                mail = mail,
                post = post,
                passvord = pwd,
                token = refrash_tpken
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
        /// Метод для авторизации в системе
        /// </summary>
        /// <param name="userParam">Обект предаствляющий входные данные</param>
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
                    string refrash_tpken = null;
                    var randomNumber = new byte[32];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(randomNumber);
                        refrash_tpken = Convert.ToBase64String(randomNumber);
                    }

                    user.token = refrash_tpken;
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
        /// Метод для получения данных по задаче/задачам
        /// </summary>
        /// <param name="taskParam">Обект предаствляющий входные данные</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetTasks([FromBody] TaskParam taskParam)
        {
            bool admin = false;
            User user = null;
            try
            {
                user = _appDB.DBUser.FirstOrDefault(p => p.token == taskParam.token);
                if (user != null && user != new User())
                    admin = true;
            }
            catch (Exception) {  }

            List<Task> array = new List<Task>();

            try
            {
                if (taskParam.filterTasks != null)
                {
                    switch (taskParam.filterTasks)
                    {
                        case "Выполненные":
                            array = _task.GetAllTasks.Where(p => p.status != "Завершен" && p.prijId == taskParam.prijId).OrderBy(p => p.planFinishDate.Date).ToList();
                            break;
                        case "Сначала текущие":
                            array.AddRange(_task.GetAllTasks.Where(p => p.status == "В работе" && p.prijId == taskParam.prijId).OrderBy(p => p.planStartDate.Date).ToList());
                            array.AddRange(_task.GetAllTasks.Where(p => p.status != "В работе" && p.prijId == taskParam.prijId).OrderBy(p => p.planStartDate.Date).ToList());
                            break;
                        case "Только текущие":
                            array = _task.GetAllTasks.Where(p => p.status == "В работе" && p.prijId == taskParam.prijId).OrderBy(p => p.planStartDate.Date).ToList();
                            break;
                        case "Не хватает материалов или в ожидании поставки":
                            array = _task.GetAllTasks.Where(p => (p.status == "Не хватает материалов" || p.status == "В ожидании поставки материалов") && p.prijId == taskParam.prijId).OrderBy(p => p.planStartDate.Date).ToList();
                            break;
                    }
                    if (array.Count() != 0) array = array.Distinct().ToList();
                    else
                    {
                        return new JsonResult(new ObjectResult(new { message = "Совпадений не найдено" }) { StatusCode = 200 });
                    }
                }

                if (taskParam.filterSupervisor != null)
                {
                    List<Task> superArray = new List<Task>();
                    foreach(var super in taskParam.filterSupervisor)
                    {
                        if(super.Trim() != "")
                        {
                            superArray.AddRange(array.Where(p => p.supervisor == super.Trim() && p.prijId == taskParam.prijId).ToList());
                        }
                    }
                    if (superArray.Count() != 0) array = superArray.Distinct().ToList();
                }

                if(taskParam.filterTasks != null && taskParam.filterSupervisor == null)
                {
                    array = _task.GetAllTasks.Where(p => p.prijId == taskParam.prijId).ToList();
                }

            }
            catch (Exception) { array = _task.GetAllTasks.Where(p => p.prijId == taskParam.prijId).ToList(); }

            var materialWrning = false;
            foreach (var task in array)
            {
                if(task.status == "Не хватает материалов")
                {
                    materialWrning = true;
                }
            }
            try
            {
                var materials = _material.GetAllMaterials.Where(p => (p.planDeliveryDate - DateTime.Now) <= TimeSpan.FromDays(7) && (p.status == "Не хватает" || p.status == "В ожидании")).ToList();

                if (materials.Count() != 0) materialWrning = true;
            }
            catch (Exception) { }
            
            var outt = new
            {
                admin = admin,
                materialWrning = materialWrning,
                tasks = array
            };

            return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });

        }

        /// <summary>
        /// Метод для создания задачи
        /// </summary>
        /// <param name="taskParam">Обект предаствляющий входные данные</param>
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
            try
            {
                factStartDate = DateTime.Parse(taskParam.factStartDate);
            }
            catch (Exception) { }
            try
            {
                planFinishDate = DateTime.Parse(taskParam.planFinishDate);
            }
            catch (Exception) { }
            try
            {
                factFinishDate = DateTime.Parse(taskParam.factFinishDate);
            }
            catch (Exception) { }
            try
            {
                planPayDate = DateTime.Parse(taskParam.planPayDate);
            }
            catch (Exception) { }
            try
            {
                factPayDate = DateTime.Parse(taskParam.factPayDate);
            }
            catch (Exception) { }

            var photo = _proj.GetProj(taskParam.prijId);
            
            var name = taskParam.name.Trim();
            var prijId = taskParam.prijId;
            var supervisor = taskParam.supervisor.Trim();
            var status = taskParam.status.Trim();
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
                status = status,
                photoPath = photo.photoPath,
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
        /// Метод дял редактирования данных по задаче
        /// </summary>
        /// <param name="taskParam">Обект предаствляющий входные данные</param>
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

            if (result.status == "Создана")
            {
                return new JsonResult(new ObjectResult(new { message = "Изменять выполненные задачи нельзя!" }) { StatusCode = 400 });
            }

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
            try
            {
                factStartDate = DateTime.Parse(taskParam.factStartDate);
            }
            catch (Exception) { }
            try
            {
                planFinishDate = DateTime.Parse(taskParam.planFinishDate);
            }
            catch (Exception) { }
            try
            {
                factFinishDate = DateTime.Parse(taskParam.factFinishDate);
            }
            catch (Exception) { }
            try
            {
                planPayDate = DateTime.Parse(taskParam.planPayDate);
            }
            catch (Exception) { }
            try
            {
                factPayDate = DateTime.Parse(taskParam.factPayDate);
            }
            catch (Exception) { }

            if (!_task.RedactToDb(taskParam.id, taskParam.name, taskParam.status, planStartDate, factStartDate, planFinishDate, factFinishDate, planPayDate, factPayDate, taskParam.planedPrice, taskParam.factPrice, taskParam.parentTaskName, taskParam.materials, taskParam.necesseMaterials, taskParam.supervisor, taskParam.planMaterialPrice, taskParam.factMaterialPrice))
            {
                return new JsonResult(new ObjectResult(new { message = "Нет возможности изменить нанные! Проверьте связи с задачей или материалами!" }) { StatusCode = 400 });
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
        /// Метод для полечения данных графика по проекту
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGraficData([FromBody] ProjParam projParam)
        {
            // get data
            var material = _appDB.DBMaterial.Where(p=>p.projId == projParam.id);
            int planMatCount = 0;
            int factMatCount = 0;
            int planMatPrice = 0;
            int factMatPrice = 0;

            var tasks = _appDB.DBTask.Where(p => p.prijId == projParam.id);
            int planWorkPrice = 0;
            int factWorkPrice = 0;

            int planTask = 0;
            int factTask = 0;

            int completTask = 0;
            int countTask = 0;
            try
            {
                planMatCount = material.Select(p => p.planCount).Sum();
            }
            catch (Exception) { }
            try
            {
                factMatCount = material.Select(p => p.factCount).Sum();
            }
            catch (Exception) { }
            try
            {
                planMatPrice = material.Select(p => p.planPrice).Sum();
            }
            catch (Exception) { }
            try
            {
                factMatPrice = material.Select(p => p.factPrice).Sum();
            }
            catch (Exception) { }

            try
            {
                planWorkPrice = tasks.Select(p => p.planedPrice).Sum();
            }
            catch (Exception) { }
            try
            {
                factWorkPrice = tasks.Select(p => p.factPrice).Sum();
            }
            catch (Exception) { }
            try
            {
                planTask = tasks.Where(p => p.planFinishDate <= DateTime.Now).ToList().Count();
            }
            catch (Exception) { }
            try
            {
                factTask = tasks.Where(p => p.factFinishDate <= DateTime.Now).ToList().Count();
            }
            catch (Exception) { }

            try
            {
                completTask = tasks.Where(p => p.status == "Выполнена").Count();
            }
            catch (Exception) { }
            try
            {
                countTask = tasks.Count();
            }
            catch (Exception) { }



            var outt = new
            {
                planMaterial = planMatCount,
                factMaterial = factMatCount,
                planMatPrise = planMatPrice,
                factMatPrise = factMatPrice,
                planWorkPrice = planWorkPrice,
                factWorkPrice = factWorkPrice,
                planTask = planTask,
                factTask = factTask,
                completedTask = completTask,
                allTask = countTask
            };

            return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });
        }


        /// <summary>
        /// Метод для получения данных по проекту/проектам
        /// </summary>
        /// <param name="projParam"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetProj([FromBody] ProjParam projParam)
        {
            bool admin = false;
            User user = null;
            try
            {
                user = _appDB.DBUser.FirstOrDefault(p => p.token == projParam.token);
                if (user != null && user != new User())
                    admin = true;
            }
            catch (Exception) { }


            if (projParam.id != 0)
            {
                Project proj = null;
                try
                {
                    proj = _proj.GetProj(projParam.id);
                }
                catch (Exception)
                {
                    return new JsonResult(new ObjectResult(new { message = "Проект не найденv!" }) { StatusCode = 400 });
                }
                var outt = new
                {
                    project = proj
                };

                return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });
            }
            else
            {
                List<Project> array = new List<Project>();

                try
                {
                    if (projParam.filter != null)
                    {
                        switch (projParam.filter)
                        {
                            case "Ближайшие по дате начала":
                                array = _proj.GetAllProjects.Where(p => p.status != "Завершен").OrderBy(p => p.planStartDate.Date).ToList();
                                break;
                            case "Сначала просроченые":
                                array.AddRange(_proj.GetAllProjects.Where(p => p.status != "Завершен" && (p.planFinishDate - DateTime.Now) <= TimeSpan.FromDays(7)).ToList());
                                array.AddRange(_proj.GetAllProjects.Where(p => p.status != "Завершен" && (p.planFinishDate - DateTime.Now) > TimeSpan.FromDays(7)).ToList());
                                break;
                            case "Ближайшие по дате завершения":
                                array = _proj.GetAllProjects.Where(p => p.status != "Завершен").OrderBy(p => p.planFinishDate.Date).ToList();
                                break;
                            case "Выполненные":
                                array = _proj.GetAllProjects.Where(p => p.status == "Завершен").OrderBy(p => p.planStartDate.Date).ToList();
                                break;
                            case "Активные":
                                array = _proj.GetAllProjects.Where(p => p.status != "Создан" && p.status != "Выпоолнена").OrderBy(p => p.planStartDate.Date).ToList();
                                break;
                        }
                        if (array.Count() != 0) array = array.Distinct().ToList();
                        else
                        {
                            return new JsonResult(new ObjectResult(new { message = "Совпадений не найдено" }) { StatusCode = 200 });
                        }
                    }
                    else
                    {
                        array = _proj.GetAllProjects;
                    }

                }
                catch (Exception) { array = _proj.GetAllProjects; }

                var outt = new
                {
                    admin = admin,
                    projects = array
                };

                return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });
            }

        }


        /// <summary>
        /// Метод для создания проекта
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
            try
            {
                factStartDate = DateTime.Parse(projParam.factStartDate);
            }
            catch (Exception) { }
            try
            {
                planFinishDate = DateTime.Parse(projParam.planFinishDate);
            }
            catch (Exception) { }
            try
            {
                factFinishDate = DateTime.Parse(projParam.factFinishDate);
            }
            catch (Exception) { }


            var name = projParam.name.Trim();
            var adr = projParam.adr.Trim();
            var supervisor = projParam.supervisor.Trim();
            var pgotoPath = projParam.photoPath.Trim();

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
                factMaterialPrice = factMaterialPrice,
                photoPath = pgotoPath,
                status = "Создан"
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
        /// Метод дял редактирования поректа
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
            try
            {
                factStartDate = DateTime.Parse(projParam.factStartDate);
            }
            catch (Exception) { }
            try
            {
                planFinishDate = DateTime.Parse(projParam.planFinishDate);
            }
            catch (Exception) { }
            try
            {
                factFinishDate = DateTime.Parse(projParam.factFinishDate);
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
                projParam.status,
                projParam.adr,
                projParam.supervisor,
                projParam.planWorkPrice,
                projParam.factWorkPrice,
                projParam.planMaterialPrice,
                projParam.factMaterialPrice,
                projParam.photoPath
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
        /// Метод для получения  информации по материалу/материалам
        /// </summary>
        /// <param name="materialParam"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMaterial([FromBody] MaterialParam materialParam)
        {
            bool admin = false;
            User user = null;
            try
            {
                user = _appDB.DBUser.FirstOrDefault(p => p.token == materialParam.token);
                if (user != null && user != new User())
                    admin = true;
            }
            catch (Exception) { }

            if (materialParam.taskId == 0)
            {
                List<Material> array = _material.GetAllMaterials;

                try
                {

                    try
                    {//category
                        if (materialParam.filterCategory != null && materialParam.filterCategory.Count() >= 1)
                        {
                            array = array.Where(p => materialParam.filterCategory.Contains(p.category)).ToList();
                        }
                    }
                    catch (Exception) { }

                    try
                    {//provider
                        if (materialParam.filterProvider != null && materialParam.filterProvider.Count() >= 1)
                        {
                            array = array.Where(p => materialParam.filterProvider.Contains(p.provider)).ToList();
                        }
                    }
                    catch (Exception) { }

                    try
                    {//task
                        if (materialParam.filterTask != null && materialParam.filterTask.Count() >= 1)
                        {
                            array = array.Where(p => materialParam.filterTask.Contains(p.taskName)).ToList();
                        }
                    }
                    catch (Exception) { }

                    try
                    {
                        if (materialParam.filterDate == "Сначала ближайшие")
                        {
                            array = array.OrderBy(p => p.planDeliveryDate.Date).ToList();
                        }
                    }
                    catch (Exception) { }




                    if (array.Count() != 0) array = array.Distinct().ToList();
                    else
                    {
                        return new JsonResult(new ObjectResult(new { message = "Совпадений не найдено" }) { StatusCode = 400 });
                    }


                }
                catch (Exception) { array = _material.GetAllMaterials; }
                var outt = new
                {
                    admin = admin,
                    materials = array
                };

                return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });
            }
            else
            {
                List<Material> array = _material.GetAllMaterials.Where(p => p.taskId == materialParam.taskId).ToList();

                var outt = new
                {
                    admin = admin,
                    materials = array
                };

                return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });
            }

        }

        /// <summary>
        /// Метод для добавления материала
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
            try
            {
                factPayDate = DateTime.Parse(materialParam.factPayDate);
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
        /// Метод для редактирования материала
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
            try
            {
                factPayDate = DateTime.Parse(materialParam.factPayDate);
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
