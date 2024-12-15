using Microsoft.AspNetCore.Mvc;
using Jeopardy.Models;
using Jeopardy.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Jeopardy.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly JeopardyContext _context;

        public AdminPanelController(JeopardyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UsersList()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult GamesList()
        {
            var games = _context.Games.ToList();
            return View(games);
        }

        public IActionResult EditGame(int id)
        {
            var game = _context.Games.Find(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> EditGame(Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Update(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GamesList));
            }
            return View(game);
        }

        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GamesList));
        }

        public IActionResult CreateUser()
        {
            ViewBag.Games = _context.Games.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
        
                var game = await _context.Games.FindAsync(user.GameId);
                if (game != null)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(UsersList));
                }
                else
                {
                    ModelState.AddModelError("GameId", "The chosen game does not exist.");
                }
            }

            ViewBag.Games = _context.Games.ToList();
            return View(user);
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UsersList));
        }

        public IActionResult EditUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(UsersList));
            }
            return View(user);
        }
    }
}