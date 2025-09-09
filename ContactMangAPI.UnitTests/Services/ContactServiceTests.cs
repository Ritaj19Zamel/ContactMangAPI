using Azure;
using ContactMangAPI.Data;
using ContactMangAPI.DTOs;
using ContactMangAPI.Models;
using ContactMangAPI.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ContactMangAPI.UnitTests.Services
{
    [TestFixture]
    class ContactServiceTests
    {
        private ContactService _service;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new ContactService(_context);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
        [Test]
        public async Task AddContactAsync_ShouldAdd_WhenValid()
        {
            var dto = new ContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@gmail.com",
                PhoneNumber = "+2001014165320",
                BirthDate = new DateTime(1995, 5, 1)
            };
            var result = await _service.AddContactAsync(1, dto);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Contact added successfully.", result.Message);
        }
        [Test]
        public async Task AddContactAsync_ShouldFail_WhenDuplicateEmail()
        {
            var dto = new ContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@gmail.com",
                PhoneNumber = "+2001014165320",
                BirthDate = new DateTime(1995, 5, 1)
            };
            await _service.AddContactAsync(1, dto);
            var result = await _service.AddContactAsync(1, dto);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Contact with the same email already exists.", result.Message);

        }
        [Test]
        public async Task AddContactAsync_ShouldFail_WhenBirthDateInFuture()
        {
            var dto = new ContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@gmail.com",
                PhoneNumber = "+2001014165320",
                BirthDate = DateTime.UtcNow.AddYears(2)
            };
            var result = await _service.AddContactAsync(1, dto);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Birthdate cannot be in the future.", result.Message);

        }
        [Test]
        public async Task GetContactByIdAsync_ShouldReturnOneContact_WhenCall()
        {
            var dto = new ContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@gmail.com",
                PhoneNumber = "+2001014165320",
                BirthDate = new DateTime(1995, 5, 1)
            };
            var addResult = await _service.AddContactAsync(1, dto);
            var contact = (Contact)addResult.Data;
            var getResult = await _service.GetContactByIdAsync(1, contact.Id);
            Assert.IsTrue(getResult.Success);
            Assert.AreEqual(contact.Id, ((Contact)getResult.Data).Id);


        }
        [Test]
        public async Task GetContactByIdAsync_ShouldFail_WhenNotFound()
        {
            var getResult = await _service.GetContactByIdAsync(1, 999);
            Assert.IsFalse(getResult.Success);
            Assert.AreEqual("Contact not found.", getResult.Message);
        }
        [Test]
        public async Task GetContactsAsync_ShouldReturnContacts_WhenCall()
        {
            var contactsToAdd = new[]
            {
                new ContactDto { FirstName = "Alice", LastName = "Zeid", Email = "a@example.com", PhoneNumber = "111", BirthDate = new DateTime(1990,1,1) },
                new ContactDto { FirstName = "Bob", LastName = "Ali", Email = "b@example.com", PhoneNumber = "222", BirthDate = new DateTime(1995,5,5) },
                new ContactDto { FirstName = "Charlie", LastName = "Mohamed", Email = "c@example.com", PhoneNumber = "333", BirthDate = new DateTime(2000,10,10) },
            };

            foreach (var dto in contactsToAdd)
                await _service.AddContactAsync(1, dto);
            var result = await _service.GetContactsAsync(1, sortBy: "FirstName", sortOrder: "asc", pageNumber: 1, pageSize: 2);
            Assert.IsTrue(result.Success);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result.Data);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ContactListResult>(json)!;

            Assert.AreEqual(3, data.TotalRecords);
            Assert.AreEqual(1, data.PageNumber);
            Assert.AreEqual(2, data.PageSize);
            Assert.AreEqual(2, data.Contacts.Count);
            Assert.AreEqual("Alice", data.Contacts[0].FirstName);
            Assert.AreEqual("Bob", data.Contacts[1].FirstName);
        }
        [Test]
        public async Task GetContactsAsync_ShouldFail_WhenNoContacts()
        {
            var result = await _service.GetContactsAsync(999);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("No contacts found", result.Message);
        }
        [Test]
        public async Task UpdateContactAsync_ShouldUpdate_WhenValis()
        {
            var dto = new ContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@gmail.com",
                PhoneNumber = "+2001014165320",
                BirthDate = new DateTime(1995, 5, 1)
            };
            var addResult = await _service.AddContactAsync(1, dto);
            var contact = (Contact)addResult.Data!;
            var updateDto = new ContactDto
            {
                FirstName = "Johnny",
                LastName = "Doe",
                Email = "johnny@gmail.com",
                PhoneNumber = "+2001014165321",
                BirthDate = new DateTime(1995, 5, 1)
            };
            var result = await _service.UpdateContactAsync(1, contact.Id, updateDto);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Contact updated successfully.", result.Message);
            var updatedContact = (Contact)result.Data!;
            Assert.AreEqual("Johnny", updatedContact.FirstName);
            Assert.AreEqual("johnny@gmail.com", updatedContact.Email);

        }
        [Test]
        public async Task UpdateContactAsync_ShouldFail_WhenContactNotFound()
        {
            var dto = new ContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@gmail.com",
                PhoneNumber = "+2001014165320",
                BirthDate = new DateTime(1995, 5, 1)
            };
            var result = await _service.UpdateContactAsync(1, 999, dto);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Contact not found.", result.Message);
        }
        [Test]
        public async Task UpdateContactAsync_ShouldFail_WhenDuplicateEmail()
        {
            var dto1 = new ContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@gmail.com",
                PhoneNumber = "+2001014165320",
                BirthDate = new DateTime(1995, 5, 1)
            };
            var dto2 = new ContactDto
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = "Alice@gmail.com",
                PhoneNumber = "+2001014165320",
                BirthDate = new DateTime(1995, 5, 1)
            };
            var contact1 = (Contact)(await _service.AddContactAsync(1, dto1)).Data!;
            var contact2 = (Contact)(await _service.AddContactAsync(1, dto2)).Data!;
            var updateDto = new ContactDto { FirstName = "Alice", LastName = "Smith", Email = "john@gmail.com", PhoneNumber = "222", BirthDate = new DateTime(1992, 2, 2) };
            var result = await _service.UpdateContactAsync(1, contact2.Id, updateDto);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Another contact with the same email already exists.", result.Message);
        }
        [Test]
        public async Task UpdateContactAsync_ShouldFail_WhenBirthDateInFuture()
        {
            var dto = new ContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@gmail.com",
                PhoneNumber = "+2001014165320",
                BirthDate = new DateTime(1990, 1, 1)
            };
            var contact = (Contact)(await _service.AddContactAsync(1, dto)).Data!;
            var Updatedto = new ContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@gmail.com",
                PhoneNumber = "+2001014165320",
                BirthDate = DateTime.UtcNow.AddYears(2)
            };
            var result = await _service.UpdateContactAsync(1, contact.Id, Updatedto);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Birthdate cannot be in the future.", result.Message);
        }
        [Test]
        public async Task DeleteContactAsync_ShouldDelete_WhenExists()
        {
            var contactDto = new ContactDto { FirstName = "John", LastName = "Doe", Email = "john@gmail.com", PhoneNumber = "111", BirthDate = new DateTime(1990, 1, 1) };
            var contact = (Contact)(await _service.AddContactAsync(1, contactDto)).Data!;

            var result = await _service.DeleteContactAsync(1, contact.Id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Contact deleted successfully.", result.Message);

            var getResult = await _service.GetContactByIdAsync(1, contact.Id);
            Assert.IsFalse(getResult.Success);
            Assert.AreEqual("Contact not found.", getResult.Message);
        }

        [Test]
        public async Task DeleteContactAsync_ShouldFail_WhenNotFound()
        {
            var result = await _service.DeleteContactAsync(1, 999);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Contact not found.", result.Message);
        }

    }
}
