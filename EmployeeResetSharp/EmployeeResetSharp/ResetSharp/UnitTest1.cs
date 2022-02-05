using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using RestSharp;
using EmployeeResetSharp;
using Newtonsoft.Json.Linq;

namespace MSTESTRestSharp
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient(" http://localhost:4000");
        }
        private RestResponse GetEmployeeList()
        {
            RestRequest request = new RestRequest("employee", Method.Get);

            RestResponse response = client.ExecuteAsync(request).Result;
            return response;
        }

        [TestMethod]
        public void OnCallingGetAPI_ReturnEmployeeList()
        {
            RestResponse response = GetEmployeeList();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(16, employeeList.Count);
            foreach (Employee emp in employeeList)
            {
                Console.WriteLine("id: " + emp.Id + "\t" + "name: " + emp.name + "\t" + "salary: " + emp.salary);
            }
        }
        [TestMethod]
        //uc2 //
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            // Arrange
            RestRequest request = new RestRequest("/employees", Method.Post);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", "Clark");
            jObjectBody.Add("salary", "15000");
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);
            //Act
            RestResponse response = client.ExecuteAsync(request).Result;
            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual("15000", dataResponse.salary);
            System.Console.WriteLine(response.Content);
        }
    }
}
   
