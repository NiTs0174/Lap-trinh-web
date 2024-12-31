﻿using B3WebsiteBanHang.DataAccess;
using B3WebsiteBanHang.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace B3WebsiteBanHang.Repository
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);           
        }
        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id ==id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            foreach(var product in category.Products)
            {
                _context.Products.Remove(product);
            }

            //var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}