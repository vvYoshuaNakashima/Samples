using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace SampleSwaggerAPI.Controllers
{
    /// <summary>
    /// Sample Pet
    /// </summary>
    [JsonObject("pet")]
    public class Pet
    {
        /// <summary>
        /// 個別ID
        /// </summary>
        [JsonProperty("petid")]
        public int? PetId { get; set; } = null;

        /// <summary>
        /// Pets's Age
        /// </summary>
        [JsonProperty("age")]
        public int? Age { get; set; } = null;

        /// <summary>
        /// Pet's Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = null;
    }


    /// <summary>
    /// コメントが反映されます。
    /// </summary>
    public class PracticeController : ApiController
    {
        List<Pet> pets = new List<Pet>();

        /// <summary>
        /// 練習用クラスのコンストラクタ
        /// </summary>
        public PracticeController()
        {
            Pet pet = new Pet
            {
                PetId = 1,
                Age = 10,
                Name = "Fluffy"
            };

            pets.Add(pet);

            pets.Add(new Pet
            {
                PetId = 2,
                Age = 25,
                Name = "Pony"
            });
        }

        /// <summary>
        /// データのリストを取得するAPIです。 JSONP 対応しました。～?callback=xfunc を指定してください。
        /// </summary>
        /// <param name="callback">JSONP 対応 callback が入ります。</param>
        /// <returns></returns>
        // GET api/<controller>
        [HttpGet]
        [ActionName("get")]
        public Pet[] Get(string callback = null)
        {
            return pets.ToArray();
        }

        /// <summary>
        /// id を指定してリストから、１つの要素を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback">JSONP 対応 callback が入ります。</param>
        /// <returns></returns>
        // GET api/<controller>/5
        [HttpGet]
        [ActionName("get")]
        public Pet Get(int id, string callback = null)
        {
            if (!pets.Any(p => p.PetId == id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return pets.Where(p => p.PetId == id).First();
        }

        /// <summary>
        /// １つの要素を新規登録するメソッドです。基本的には、GET メソッドを使用します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // POST api/<controller>
        [HttpGet]
        [ActionName("post")]
        public Pet[] GetPost([FromUri]Pet value)
        {
            return PostMethod(value);
        }

        /// <summary>
        /// １つの要素が 2048byte を超える場合、POST メソッドを使用します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // POST api/<controller>
        [HttpPost]
        public Pet[] Post([FromBody]Pet value)
        {
            return PostMethod(value);
        }

        private Pet[] PostMethod([FromBody]Pet value)
        {
            pets.Add(value);
            return pets.ToArray();
        }

        /// <summary>
        /// １つの要素を更新する APIです。基本的には、GET メソッドを使用します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        // PUT api/<controller>/5
        [HttpGet]
        [ActionName("put")]
        public Pet[] GetPut(int id, [FromUri]Pet value)
        {
            return PutMethod(id, value);
        }

        /// <summary>
        /// １つの要素内のパラメータを個別に更新する APIです。基本的には、GET メソッドを使用します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        // PUT api/<controller>/5
        [HttpGet]
        [ActionName("put")]
        [Route("api/Practice/put/{id}/{parameter}")]
        public Pet GetPut(int id, string parameter, [FromUri]string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new HttpRequestException();
            }

            Pet pet = new Pet();
            switch (parameter)
            {
                case "petid":
                case "petId":
                case "Petid":
                case "PetId":
                    throw new HttpRequestException();

                case "age":
                case "Age":
                    if (int.TryParse(value, out int age))
                    {
                        pet.Age = age;
                    }
                    break;

                case "name":
                case "Name":
                    if (!string.IsNullOrEmpty(value))
                    {
                        pet.Name = parameter;
                    }
                    break;

                default:
                    break;
            }

            PutMethod(id, pet);
            return pets.Where(p => p.PetId == id).First();
        }

        /// <summary>
        /// １つの要素が 2048byte を超える場合、POST メソッドを使用します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        // PUT api/<controller>/5
        [HttpPost]
        [ActionName("put")]
        public Pet[] PostPut(int id, [FromBody]Pet value)
        {
            return PutMethod(id, value);
        }

        /// <summary>
        /// PUT メソッドは、使用を禁止します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        // PUT api/<controller>/5
        [HttpPut]
        public Pet[] Put(int id, [FromBody]Pet value)
        {
            return PutMethod(id, value);
        }

        private Pet[] PutMethod(int id, [FromBody]Pet value)
        {
            if (!pets.Any(p => p.PetId == id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var pet = pets.Where(p => p.PetId == id).First();
            pet.Age = value.Age ?? pet.Age;
            pet.Name = value.Name ?? pet.Name;
            return pets.ToArray();
        }

        /// <summary>
        /// １つの要素を削除する APIです。基本的には、GET メソッドを使用します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<controller>/5
        [HttpGet]
        [ActionName("delete")]
        public Pet[] GetDelete(int id)
        {
            return DeleteMethod(id);
        }

        /// <summary>
        /// DELETE HTTP メソッドは、使用を禁止します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<controller>/5
        [HttpDelete]
        public Pet[] Delete(int id)
        {
            return DeleteMethod(id);
        }

        private Pet[] DeleteMethod(int id)
        {
            if (!pets.Any(p => p.PetId == id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var pet = pets.Where(p => p.PetId == id).First();
            bool result = pets.Remove(pet);
            return pets.ToArray();
        }
    }
}