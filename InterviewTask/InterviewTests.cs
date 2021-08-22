using System.Net;
using InterviewTask.Helpers;
using InterviewTask.TestData;
using NUnit.Framework;

namespace InterviewTask
{
    [TestFixture]
    public class InterviewTests
    {
        private readonly TestHelper _testHelper = new TestHelper();

        [Test]
        public void GetAllUsers_UsersReceivedSuccessfully() // Test Cases #1, #2, #3, #4 covered.
        {
            var users = _testHelper.GetUsers(out var response);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"The request status code is {response.StatusCode}, " +
                                                                            $"but must be {HttpStatusCode.OK}");
            Assert.IsNotEmpty(users, "There are no user records on the server");
            Assert.IsTrue(_testHelper.IsNecessaryHeadersInResponse(response), "There is no some necessary header is the response.");
            Assert.IsFalse(_testHelper.IsDangerousHeadersInResponse(response), "The response contains a potentially dangerous header."); // Bug#1 and #2
        }

        [Test]
        public void CreateUser_UserCreatedSuccessfully() // Test Cases #8, #10, #11, #13 covered. 
        {
            var user = new User();

            _testHelper.RegisterUser(user, out var response);

            var newUserFromDb = _testHelper.GetUserByUsername(user.username);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"The request status code is {response.StatusCode}, " +
                                                                            $"but must be {HttpStatusCode.OK}");
            Assert.IsTrue(_testHelper.IsNecessaryHeadersInResponse(response), "There is no some necessary header is the response.");
            Assert.IsFalse(_testHelper.IsDangerousHeadersInResponse(response), "The response contains a potentially dangerous header."); // Bug#1 and #2
            Assert.That(user.username, Is.EqualTo(newUserFromDb.username), "Invalid username or user not created.");
        }

        [TestCase("TestName", "TestEmail@mail.com", "")] // Test Case #19
        [TestCase("TestName", "", "password")] // Test Case #18
        [TestCase("", "TestEmail@mail.com", "password")] // Test Case #17
        public void CreateUser_WithoutRequiredParameter_UserNotCreated(string userName, string email, string password)
        {
            var user = new User(userName, email, password);

            _testHelper.RegisterUser(user, out var response);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), $"The request status code is {response.StatusCode}," +
                                                                                    $" but must be {HttpStatusCode.BadRequest}");
            Assert.Null(_testHelper.GetUserByUsername(user.username), "User created.");
        }
    }
}