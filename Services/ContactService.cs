﻿using dotNetShop.Data;
using dotNetShop.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNetShop.Services
{
	public class ContactService : IContactService
	{
		private readonly ShopDBContext _dbContext;

		public ContactService() { }
		
		public ContactService(ShopDBContext dbContext) { 
			_dbContext = dbContext;
		}


		public async Task<Contact> GetContactAsync()
		{
			Contact contact = await _dbContext.Contacts.FirstOrDefaultAsync();
			return contact;
		}
	}
}