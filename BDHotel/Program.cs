using Microsoft.EntityFrameworkCore;
using BDHotel.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseSqlServer("Server=DESKTOP-8VL22MN\\SQLEXPRESS;Database=hhh;Trusted_Connection=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True")
                .Options;

            using var context = new HotelDbContext(options);

            // пункт 2.1
            Console.WriteLine("пункт 2.1");
            var employee = await context.Employees.FirstOrDefaultAsync();
            if (employee != null)
            {
                Console.WriteLine("Все сотрудники (первая запись):");
                Console.WriteLine($"ID: {employee.EmployeeId}, Полное имя: {employee.FullName}");
            }
            else
            {
                Console.WriteLine("Сотрудники не найдены.");
            }

            // пункт 2.2
            Console.WriteLine("\nпункт 2.2");
            var filteredEmployee = await context.Employees
                .Where(e => e.Age > 30)
                .FirstOrDefaultAsync();
            if (filteredEmployee != null)
            {
                Console.WriteLine("Сотрудники старше 30 (первая запись):");
                Console.WriteLine($"ID: {filteredEmployee.EmployeeId}, Полное имя: {filteredEmployee.FullName}, Возраст: {filteredEmployee.Age}");
            }
            else
            {
                Console.WriteLine("Сотрудники старше 30 не найдены.");
            }

            // пункт 2.3
            Console.WriteLine("\nпункт 2.3");
            var bookingStat = await context.Bookings
                .GroupBy(b => b.ClientId)
                .Select(g => new { ClientId = g.Key, BookingCount = g.Count() })
                .FirstOrDefaultAsync();
            if (bookingStat != null)
            {
                Console.WriteLine("Статистика бронирований по клиентам (первая группа):");
                Console.WriteLine($"ID клиента: {bookingStat.ClientId}, Количество бронирований: {bookingStat.BookingCount}");
            }
            else
            {
                Console.WriteLine("Статистика бронирований не найдена.");
            }

            // пункт 2.4
            Console.WriteLine("\nпункт 2.4");
            var employeeBooking = await (from e in context.Employees
                                         join b in context.Bookings on e.EmployeeId equals b.EmployeeId into bookingsGroup
                                         from b in bookingsGroup.DefaultIfEmpty()
                                         select new { e.FullName, BookingId = b != null ? b.BookingId : 0 })
                                        .FirstOrDefaultAsync();
            if (employeeBooking != null)
            {
                Console.WriteLine("Бронирования сотрудников (первая запись):");
                Console.WriteLine($"Сотрудник: {employeeBooking.FullName}, ID брони: {employeeBooking.BookingId}");
            }
            else
            {
                Console.WriteLine("Бронирования сотрудников не найдены.");
            }

            // пункт 2.5
            Console.WriteLine("\nпункт 2.5");
            var filteredEmployeeBooking = await (from e in context.Employees
                                                 join b in context.Bookings on e.EmployeeId equals b.EmployeeId into bookingsGroup
                                                 from b in bookingsGroup.DefaultIfEmpty()
                                                 where b != null && b.RoomId == 1
                                                 select new { e.FullName, BookingId = b.BookingId, RoomId = b.RoomId })
                                                .FirstOrDefaultAsync();
            if (filteredEmployeeBooking != null)
            {
                Console.WriteLine("Отфильтрованные бронирования сотрудников (Room ID = 1, первая запись):");
                Console.WriteLine($"Сотрудник: {filteredEmployeeBooking.FullName}, ID брони: {filteredEmployeeBooking.BookingId}, ID комнаты: {filteredEmployeeBooking.RoomId}");
            }
            else
            {
                Console.WriteLine("Бронирования с Room ID 1 не найдены.");
            }

            // пункт 2.6
            Console.WriteLine("\nпункт 2.6");
            var newEmployee = new Employee
            {
                FullName = "Alex Johnson",
                Age = 35,
                Gender = "Male",
                Address = "123 Main St",
                Phone = "+1234567890",
                PassportData = "AB123456",
                PositionId = 1 // Предполагаем существующий Position
            };
            try
            {
                context.Employees.Add(newEmployee);
                await context.SaveChangesAsync();
                Console.WriteLine("Добавлен новый сотрудник: Alex Johnson");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении сотрудника: {ex.InnerException?.Message ?? ex.Message}");
            }

            // пункт 2.7
            Console.WriteLine("\nпункт 2.7");
            var newBooking = new Booking
            {
                RoomId = 1,      // Предполагаем существующий Room
                ClientId = 1,    // Предполагаем существующий Client
                EmployeeId = 1,  // Предполагаем существующий Employee
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(3)
            };
            try
            {
                context.Bookings.Add(newBooking);
                await context.SaveChangesAsync();
                Console.WriteLine("Добавлена новая бронь.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении брони: {ex.InnerException?.Message ?? ex.Message}");
            }

            // пункт 2.8
            Console.WriteLine("\nпункт 2.8");
            var employeeToDelete = await context.Employees.FindAsync(8);
            try
            {
                if (employeeToDelete != null)
                {
                    // Удаляем все связанные брони
                    var relatedBookings = await context.Bookings
                        .Where(b => b.EmployeeId == employeeToDelete.EmployeeId)
                        .Include(b => b.BookingServices) // Загружаем связанные услуги
                        .ToListAsync();
                    foreach (var booking in relatedBookings)
                    {
                        context.BookingServices.RemoveRange(booking.BookingServices); // Удаляем услуги
                    }
                    context.Bookings.RemoveRange(relatedBookings); // Удаляем брони
                    context.Employees.Remove(employeeToDelete); // Удаляем сотрудника
                    await context.SaveChangesAsync();
                    Console.WriteLine("Удален сотрудник с ID 8 и все связанные брони и услуги.");
                }
                else
                {
                    Console.WriteLine("Сотрудник с ID 8 не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении сотрудника: {ex.InnerException?.Message ?? ex.Message}");
            }

            // пункт 2.9
            Console.WriteLine("\nпункт 2.9");
            var bookingToDelete = await context.Bookings.FindAsync(3);
            try
            {
                if (bookingToDelete != null)
                {
                    // Удаляем связанные услуги
                    var relatedServices = await context.BookingServices
                        .Where(bs => bs.BookingId == bookingToDelete.BookingId)
                        .ToListAsync();
                    context.BookingServices.RemoveRange(relatedServices);
                    context.Bookings.Remove(bookingToDelete);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Удалена бронь с ID 3 и связанные услуги.");
                }
                else
                {
                    Console.WriteLine("Бронь с ID 3 не найдена.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении брони: {ex.InnerException?.Message ?? ex.Message}");
            }

            // пункт 2.10
            Console.WriteLine("\nпункт 2.10");
            try
            {
                var manager = await context.Positions.FirstOrDefaultAsync(p => p.PositionName == "Manager");
                if (manager == null)
                {
                    manager = new Position { PositionName = "Manager", Salary = 50000m, Responsibilities = "Manage staff", Requirements = "Experience" };
                    context.Positions.Add(manager);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Добавлена должность Менеджер для теста.");
                }
                manager.Salary *= 1.10m;
                await context.SaveChangesAsync();
                Console.WriteLine("Зарплата для должности Менеджер увеличена на 10%.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении зарплаты: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла общая ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода.");
        Console.ReadKey();
    }
}