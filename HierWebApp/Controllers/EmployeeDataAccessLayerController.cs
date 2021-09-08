using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HierWebApp.Controllers
{
    [FirestoreData]
    public class Employee
    {
        public string EmployeeId { get; set; }
        public DateTime date { get; set; }
        [FirestoreProperty]
        public string EmployeeName { get; set; }
        [FirestoreProperty]
        public string CityName { get; set; }
        [FirestoreProperty]
        public string Designation { get; set; }
        [FirestoreProperty]
        public string Gender { get; set; }
    }
    [FirestoreData]
    public class Cities
    {
        public string CityName { get; set; }

    }
    public class EmployeeDataAccessLayerController : Controller
    {
        string projectId;
        FirestoreDb firestoreDb;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }

        public EmployeeDataAccessLayerController()
        {

            string filePath = "C:\\FirestoreAPIKey\\blazorcloudfirestore-22520-firebase-adminsdk-t6nq8-4f618a63ad.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            projectId = "blazorcloudfirestore-22520";
            firestoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<JsonResult> GetAllEmployees()
        {
            try
            {

                string filePath = "C:\\FirestoreAPIKey\\blazorcloudfirestore-22520-firebase-adminsdk-t6nq8-4f618a63ad.json";
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
                projectId = "blazorcloudfirestore-22520";
                firestoreDb = FirestoreDb.Create(projectId);

                Query employeeQuery = firestoreDb.Collection("employees");
                QuerySnapshot employeeQuerySnapshot = await employeeQuery.GetSnapshotAsync();
                List<Employee> listEmployee = new List<Employee>();
                foreach (DocumentSnapshot documentSnapshot in employeeQuerySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> city = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(city);
                        Employee newUser = JsonConvert.DeserializeObject<Employee>(json);
                        newUser.EmployeeId = documentSnapshot.Id;
                        newUser.date = documentSnapshot.CreateTime.Value.ToDateTime();
                        listEmployee.Add(newUser);
                    }
                }
                List<Employee> storedEmployeeList = listEmployee.OrderBy(x => x.date).ToList();
                return Json(storedEmployeeList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<JsonResult> Addemployee(Employee employee)
        {
            try
            {
                CollectionReference colRef = firestoreDb.Collection("employees");
                await colRef.AddAsync(employee);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<JsonResult> UpdateEmployee(Employee employee)
        {
            try
            {
                DocumentReference docRef = firestoreDb.Collection("employees").Document(employee.EmployeeId);
                await docRef.SetAsync(employee, SetOptions.Overwrite);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
        public async Task<JsonResult> GetEmployeeData(string id)
        {
            try
            {
                DocumentReference docRef = firestoreDb.Collection("employees").Document(id);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    Employee emp = snapshot.ConvertTo<Employee>();
                    emp.EmployeeId = snapshot.Id;
                    return Json(emp, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new Employee(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<JsonResult> DeleteEmployee(string id)
        {
            try
            {
                DocumentReference docRef = firestoreDb.Collection("employees").Document(id);
                await docRef.DeleteAsync();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public async Task<JsonResult> GetCityData()
        {
            try
            {
                Query citiesQuery = firestoreDb.Collection("cities");
                QuerySnapshot citySnapShot = await citiesQuery.GetSnapshotAsync();
                List<Cities> listCity = new List<Cities>();
                foreach (DocumentSnapshot ds in citySnapShot.Documents)
                {
                    if (ds.Exists)
                    {
                        Dictionary<string, object> city = ds.ToDictionary();
                        string json = JsonConvert.SerializeObject(city);
                        Cities newCity = JsonConvert.DeserializeObject<Cities>(json);
                        listCity.Add(newCity);
                    }
                }
                return Json(listCity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}