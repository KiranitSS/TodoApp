using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TodoApp_DomainEntities;

namespace TodoAppWeb.Models.Repository
{
    public class EFTodosRepository : ITodosRepository
    {
        private readonly ITodosDbContext context;

        public EFTodosRepository(ITodosDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Todo> Todos
        {
            get => context.TodoLists.Include(l => l.Todos).SelectMany(l => l.Todos);
        }

        public void CreateTodo(Todo todo)
        {
            if (todo is null)
            {
                throw new ArgumentNullException(nameof(todo));
            }

            TodoList list = context.TodoLists.FirstOrDefault(l => l.Id == todo.ListId)
                ?? throw new InvalidOperationException("No such list found.");

            context.Add(todo);
            list.Todos.Add(todo);
            context.SaveChanges();
        }

        public void DeleteTodo(Todo todo)
        {
            if (todo is null)
            {
                throw new ArgumentNullException(nameof(todo));
            }

            Todo todoToDelete = GetTodoById(todo.Id)
                ?? throw new InvalidOperationException("No such todo found.");

            context.Remove(todoToDelete);
            context.SaveChanges();
        }

        public void UpdateTodo(Todo todo)
        {
            if (todo is null)
            {
                throw new ArgumentNullException(nameof(todo));
            }

            Todo todoToUpdate = GetTodoById(todo.Id)
                ?? throw new InvalidOperationException("No such todo found.");

            todoToUpdate.Title = todo.Title;
            todoToUpdate.Description = todo.Description;
            todoToUpdate.DueDate = todo.DueDate;
            todoToUpdate.Status = todo.Status;


            this.context.SaveChanges();
        }

        public Todo? GetTodoById(long id)
        {
            return this.Todos.FirstOrDefault(t => t.Id == id);
        }
    }
}
