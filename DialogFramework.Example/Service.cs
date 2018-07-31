//Copyright © Sergei Semenkov 2018
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;

namespace DialogFramework.Example
{
    public class Service
    {
        private DialogFramework.Service service;

        public Service()
        {
            service = new DialogFramework.Service();
            service.OutgoingMessage += Service_OutgoingMessage;
        }

        private void Service_OutgoingMessage(object sender, OutgoingMessageEventArgs e)
        {
            if (OutgoingMessage != null)
                OutgoingMessage(this, e);
        }

        public event DialogFramework.Service.OutgoingMessageHandler OutgoingMessage;

        public void LoadDictionary(string fileName)
        {
            service.LoadDictionary(fileName);
        }
        public void LoadGrammar(string fileName)
        {
            service.LoadGrammar(fileName);
        }
        public void LoadDialog(string fileName)
        {
            service.LoadDialog(fileName);
        }
        public void LoadPersonsData(string fileName)
        {
            if(!String.IsNullOrEmpty(fileName))
            {
                JObject personDataJson = JObject.Parse(File.ReadAllText(fileName));
                foreach(JObject personObject in personDataJson["persons"])
                {
                    Entity entity = new Entity();
                    entity.Class = "Person";
                    entity.ID = personObject["id"].ToString();

                    string gender = personObject["gender"].ToString() == "м" ? "masc" : "femn";

                    List<TemplateItem> items = new List<TemplateItem>
                    {
                        new TemplateToken()
                        {
                            Token = new Token(personObject["name"].ToString()),
                            IsHead = true,
                            POS = "NOUN",
                            Features = new Dictionary<string, string> { { "Gender", gender }, { "Number", "sing" } }
                        }
                    };
                    entity.Properties.Add(new EntityProperty() { Entity = entity, Set = "PersonName", Name = "name", Value = personObject["name"].ToString(), Template = Template.Load(items) });

                    items = new List<TemplateItem>
                    {
                        new TemplateToken()
                        {
                            Token = new Token(personObject["surname"].ToString()),
                            IsHead = true,
                            POS = "NOUN",
                            Features = new Dictionary<string, string> { { "Gender", gender }, { "Number", "sing" } }
                        }
                    };
                    entity.Properties.Add(new EntityProperty() { Entity = entity, Set = "surname", Name = "surname", Value = personObject["surname"].ToString(), Template = Template.Load(items) });

                    items = new List<TemplateItem>
                    {
                        new TemplateToken()
                        {
                            Token = new Token(personObject["patronymic"].ToString()),
                            IsHead = true,
                            POS = "NOUN",
                            Features = new Dictionary<string, string> { { "Gender", gender }, { "Number", "sing" } }
                        }
                    };
                    entity.Properties.Add(new EntityProperty() { Entity = entity, Set = "patronymic", Name = "patronymic", Value = personObject["patronymic"].ToString(), Template = Template.Load(items) });

                    string fio = $"{personObject["surname"]} {personObject["name"]} {personObject["patronymic"]}";
                    List<TemplateItem> fioItems = new List<TemplateItem> {
                        new TemplateToken()
                        {
                            Id = "head",
                            IsHead = true,
                            IsVirtual = true,
                            POS = "NOUN",
                            Features = new Dictionary<string, string> { { "Gender", gender }, { "Number", "sing" } }
                        } ,
                        new TemplateToken()
                        {
                            LemmaRef = new LemmaRef()
                            {
                                Token = new Token(personObject["surname"].ToString()),
                                POS = "NOUN",
                                Features = new Dictionary<string, string> { { "Gender", gender }, { "Number", "sing" }, { "Case", "nomn" } }
                            },
                            SyncTo = new SyncTo() { Target = "head", Features = new List<string> { "Case" } }
                        },
                        new TemplateToken()
                        {
                            LemmaRef = new LemmaRef()
                            {
                                Token = new Token(personObject["name"].ToString()),
                                POS = "NOUN",
                                Features = new Dictionary<string, string> { { "Gender", gender }, { "Number", "sing" }, { "Case", "nomn" } }
                            },
                            SyncTo = new SyncTo() { Target = "head", Features = new List<string> { "Case" } }
                        },
                        new TemplateToken()
                        {
                            LemmaRef = new LemmaRef()
                            {
                                Token = new Token(personObject["patronymic"].ToString()),
                                POS = "NOUN",
                                Features = new Dictionary<string, string> { { "Gender", gender }, { "Number", "sing" }, { "Case", "nomn" } }
                            },
                            SyncTo = new SyncTo() { Target = "head", Features = new List<string> { "Case" } }
                        }
                    };
                    entity.Properties.Add(new EntityProperty() { Entity = entity, Set = "Person", Name = "fio", Value = fio, Template = Template.Load(fioItems) });

                    entity.Properties.Add(new EntityProperty() { Entity = entity, Set = "Phone", Name = "phone", Value = personObject["phone"].ToString(), Template = Template.Load(personObject["phone"].ToString()) });

                    entity.Properties.Add(new EntityProperty() { Entity = entity, Class = "Department", Name = "department", Value = personObject["departmentId"].ToString() });

                    entity.Properties.Add(new EntityProperty() { Entity = entity, Set = "Office", Name = "office", Value = personObject["office"].ToString(), Template = Template.Load(personObject["office"].ToString()) });

                    entity.Properties.Add(new EntityProperty() { Entity = entity, Set = "email", Name = "email", Value = personObject["email"].ToString(), Template = Template.Load(personObject["email"].ToString()) });
                }
            }
        }
        public void LoadDepartmentsData(string fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                JObject departmentDataJson = JObject.Parse(File.ReadAllText(fileName));
                foreach (JObject departmentObject in departmentDataJson["departments"])
                {
                    Entity entity = new Entity();
                    entity.Class = "Department";
                    entity.ID = departmentObject["id"].ToString();

                    entity.Properties.Add(new EntityProperty() { Entity = entity, Name = "name", Value = departmentObject["name"].ToString(), Set = "Department", Template = Template.Load(departmentObject["name"].ToString()) });

                    entity.Properties.Add(new EntityProperty() { Entity = entity, Class = "Person", Name = "person", Value = departmentObject["headOfDepartmentId"].ToString() });
                }
            }
        }

        public void Proccess(string text)
        {
            service.ProccessMessage(new ChatId() { Chanel = "Console App", Chat = "Console App" }, text);
        }
    }
}
