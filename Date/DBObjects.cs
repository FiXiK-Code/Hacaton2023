using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVP.Date
{
    public class DBObjects
    {
        public static void Initial(AppDB content)
        {
            if (!content.DBMaterial.Any())
                content.DBMaterial.AddRange(Material.Select(p => p.Value));

            if (!content.DBProject.Any())
                content.DBProject.AddRange(Project.Select(p => p.Value));

            if (!content.DBTask.Any())
                content.DBTask.AddRange(Task.Select(p => p.Value));

            if (!content.DBUser.Any())
                content.DBUser.AddRange(User.Select(p => p.Value));

            content.SaveChanges();
        }
        
        public static Dictionary<string, Material> _Material;
        public static Dictionary<string, Material> Material
        {
            get
            {

                if (_Material == null)
                {
                    var list = new Material[]
                    {
                        new Material
                        {
                            name = "derevo",
                            category = "drevesina",
                            countName = "m2",
                            planCount = 200,
                            factCount = 0,
                            planPrice = 100000,
                            factPrice = 0,
                            status = "Нет в наличии",
                            planPayDate = new DateTime(2023,10,1),
                            factPayDate = new DateTime(1,1,1),
                            taskId = 0,
                            taskName = null,
                            projId = 0,
                            provider = "OOO AAA"
                        },
                        new Material
                        {
                            name = "armatura",
                            category = "metal",
                            countName = "m2",
                            planCount = 200,
                            factCount = 0,
                            planPrice = 100000,
                            factPrice = 0,
                            status = "Нет в наличии",
                            planPayDate = new DateTime(2023,10,1),
                            factPayDate = new DateTime(1,1,1),
                            taskId = 0,
                            taskName = null,
                            projId = 0,
                            provider = "OOO AAA"
                        },
                        new Material
                        {
                            name = "beton",
                            category = "cement",
                            countName = "m2",
                            planCount = 200,
                            factCount = 0,
                            planPrice = 100000,
                            factPrice = 0,
                            status = "Нет в наличии",
                            planPayDate = new DateTime(2023,10,1),
                            factPayDate = new DateTime(1,1,1),
                            taskId = 0,
                            taskName = null,
                            projId = 0,
                            provider = "OOO AAA"
                        },
                        new Material
                        {
                            name = "doski",
                            category = "drevesina",
                            countName = "m2",
                            planCount = 200,
                            factCount = 0,
                            planPrice = 100000,
                            factPrice = 0,
                            status = "Нет в наличии",
                            planPayDate = new DateTime(2023,10,1),
                            factPayDate = new DateTime(1,1,1),
                            taskId = 0,
                            taskName = null,
                            projId = 0,
                            provider = "OOO AAA"
                        },
                    };
                    _Material = new Dictionary<string, Material>();
                    foreach (Material el in list)
                    {
                        _Material.Add(el.name, el);
                    }
                }
                return _Material;
            }
        }

        public static Dictionary<string, Project> _Project;
        public static Dictionary<string, Project> Project
        {
            get
            {

                if (_Project == null)
                {
                    var list = new Project[]
                    {
                        new Project
                        {
                            name = "proj 1",
                            adr = "adrest proecta 1",
                            supervisor = " OOO supervisor 1",
                            planStartDate = new DateTime(2023,10,1),
                            factStartDate = new DateTime(1,1,1),
                            planFinishDate = new DateTime(2023,10,1),
                            factFinishDate = new DateTime(1,1,1),
                            planWorkPrice = 1000000,
                            factWorkPrice = 0,
                            planMaterialPrice = 500000,
                            factMaterialPrice = 0
                        },
                        new Project
                        {
                            name = "proj 2",
                            adr = "adrest proecta 2",
                            supervisor = " OOO supervisor 2",
                            planStartDate = new DateTime(2023,10,1),
                            factStartDate = new DateTime(1,1,1),
                            planFinishDate = new DateTime(2023,10,1),
                            factFinishDate = new DateTime(1,1,1),
                            planWorkPrice = 1000000,
                            factWorkPrice = 0,
                            planMaterialPrice = 500000,
                            factMaterialPrice = 0
                        },
                        new Project
                        {
                            name = "proj 3",
                            adr = "adrest proecta 3",
                            supervisor = " OOO supervisor 3",
                            planStartDate = new DateTime(2023,10,1),
                            factStartDate = new DateTime(1,1,1),
                            planFinishDate = new DateTime(2023,10,1),
                            factFinishDate = new DateTime(1,1,1),
                            planWorkPrice = 1000000,
                            factWorkPrice = 0,
                            planMaterialPrice = 500000,
                            factMaterialPrice = 0
                        },
                        new Project
                        {
                            name = "proj 4",
                            adr = "adrest proecta 4",
                            supervisor = " OOO supervisor 4",
                            planStartDate = new DateTime(2023,10,1),
                            factStartDate = new DateTime(1,1,1),
                            planFinishDate = new DateTime(2023,10,1),
                            factFinishDate = new DateTime(1,1,1),
                            planWorkPrice = 1000000,
                            factWorkPrice = 0,
                            planMaterialPrice = 500000,
                            factMaterialPrice = 0
                        },
                    };
                    _Project = new Dictionary<string, Project>();
                    foreach (var el in list)
                    {
                        _Project.Add(el.name, el);
                    }
                }
                return _Project;
            }
        }

        public static Dictionary<string, Task> _Task;
        public static Dictionary<string, Task> Task
        {
            get
            {

                if (_Task == null)
                {
                    var list = new Task[]
                    {
                        new Task
                        {
                            name = "task 1",
                            prijId = 1,
                            supervisor = "OOO taskSupervisor 1",
                            planStartDate = new DateTime(2023,10,1),
                            factStartDate = new DateTime(1,1,1),
                            planFinishDate = new DateTime(2023,10,1),
                            factFinishDate = new DateTime(1,1,1),
                            planPayDate = new DateTime(2023,10,1),
                            factPayDate = new DateTime(1,1,1),
                            planedPrice = 1000000,
                            factPrice = 0,
                            parentTaskName = null,
                            parentTaskId = 0,
                            materials = "armatura, derevo",
                            necesseMaterials = "derevo",
                            planedMaterialPrice = 500000,
                            factMaterialPrice = 0
                        },
                        new Task
                        {
                            name = "task 2",
                            prijId = 2,
                            supervisor = "OOO taskSupervisor 2",
                            planStartDate = new DateTime(2023,10,1),
                            factStartDate = new DateTime(1,1,1),
                            planFinishDate = new DateTime(2023,10,1),
                            factFinishDate = new DateTime(1,1,1),
                            planPayDate = new DateTime(2023,10,1),
                            factPayDate = new DateTime(1,1,1),
                            planedPrice = 1000000,
                            factPrice = 0,
                            parentTaskName = null,
                            parentTaskId = 0,
                            materials = "armatura, derevo",
                            necesseMaterials = "derevo",
                            planedMaterialPrice = 500000,
                            factMaterialPrice = 0
                        },
                        new Task
                        {
                            name = "task 3",
                            prijId = 3,
                            supervisor = "OOO taskSupervisor 3",
                            planStartDate = new DateTime(2023,10,1),
                            factStartDate = new DateTime(1,1,1),
                            planFinishDate = new DateTime(2023,10,1),
                            factFinishDate = new DateTime(1,1,1),
                            planPayDate = new DateTime(2023,10,1),
                            factPayDate = new DateTime(1,1,1),
                            planedPrice = 1000000,
                            factPrice = 0,
                            parentTaskName = null,
                            parentTaskId = 0,
                            materials = "armatura, derevo",
                            necesseMaterials = "derevo",
                            planedMaterialPrice = 500000,
                            factMaterialPrice = 0
                        },
                        new Task
                        {
                            name = "task 4",
                            prijId = 4,
                            supervisor = " OOO taskSupervisor 4",
                            planStartDate = new DateTime(2023,10,1),
                            factStartDate = new DateTime(1,1,1),
                            planFinishDate = new DateTime(2023,10,1),
                            factFinishDate = new DateTime(1,1,1),
                            planPayDate = new DateTime(2023,10,1),
                            factPayDate = new DateTime(1,1,1),
                            planedPrice = 1000000,
                            factPrice = 0,
                            parentTaskName = null,
                            parentTaskId = 0,
                            materials = "armatura, derevo",
                            necesseMaterials = "derevo",
                            planedMaterialPrice = 500000,
                            factMaterialPrice = 0
                        },
                    };
                    _Task = new Dictionary<string, Task>();
                    foreach (var el in list)
                    {
                        _Task.Add(el.name, el);
                    }
                }
                return _Task;
            }
        }

        public static Dictionary<string, User> _User;
        public static Dictionary<string, User> User
        {
            get
            {

                if (_User == null)
                {
                    var list = new User[]
                    {
                        new User
                        {
                            name = "user 1",
                            seurname = "user1seurname",
                            mail = "user1@mail.ru",
                            post = "ingener",
                            passvord = "123456",
                            token = null
                        },
                        new User
                        {
                            name = "user 2",
                            seurname = "user2seurname",
                            mail = "user2@mail.ru",
                            post = "builder",
                            passvord = "123456",
                            token = null
                        },
                        new User
                        {
                            name = "user 3",
                            seurname = "user3seurname",
                            mail = "user3@mail.ru",
                            post = "director",
                            passvord = "123456",
                            token = null
                        },
                        new User
                        {
                            name = "user 4",
                            seurname = "user4seurname",
                            mail = "user4@mail.ru",
                            post = "prorab",
                            passvord = "123456",
                            token = null
                        }
                    };
                    _User = new Dictionary<string, User>();
                    foreach (var el in list)
                    {
                        _User.Add(el.name, el);
                    }
                }
                return _User;
            }
        }
    }
}
