using Microsoft.AspNetCore.Mvc;
using StoreFlow.Context;
using StoreFlow.Entities;

namespace StoreFlow.Controllers
{
    public class TodoController : Controller
    {
        private readonly StoreContext _context;

        public TodoController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateToDo()
        {
            var todos = new List<Todo>
            {
                new Todo{ Description="Mail gönder",Status=true,Priority="Birincil"},
                new Todo{ Description="Rapor Hazırla",Status=true,Priority="İkincil"},
                new Todo{ Description="Toplantıya Katıl",Status=true,Priority="Üçüncül"}
            };
            await _context.Todos.AddRangeAsync(todos);
            await _context.SaveChangesAsync();
            return View();
        }

        public IActionResult ToDoAggregatePrioriy()
        {
            var priorityFirtslyTodo = _context.Todos.Where(t => t.Priority == "Birincil").Select(y => y.Description).ToList();

            string result = priorityFirtslyTodo.Aggregate((acc, desc) => acc + " , " + desc);
            ViewBag.result = result;
            return View();
        }

        public IActionResult IncompletedTask()
        {
            var values = _context.Todos.Where(t => t.Status == false).Select(x => x.Description).ToList()
                .Append("Gün Sonunda Tüm Görevleri Kontrol Etmeyi Unutmayın ..!").ToList();


            return View(values);
        }

        public IActionResult ToDoChunk()
        {
            var vals = _context.Todos.Where(x=>!x.Status).ToList().Chunk(2).ToList();
             
            return View(vals);
        }

        public IActionResult ToDoConcat()
        {
            var vals = _context.Todos.Where(x=>x.Priority=="Birincil").ToList().
                Concat(_context.Todos.Where(y=>y.Priority=="İkincil").ToList()).ToList();
            return View(vals);
        }

        public IActionResult ToDoUnion()
        {
            var vals = _context.Todos.Where(x => x.Priority == "Birincil").ToList();
            var vasl2= _context.Todos.Where(y => y.Priority == "İkincil").ToList();
            var result= vals.UnionBy(vasl2,x=>x.Description).ToList();

            return View(result);
        }
    }
}
