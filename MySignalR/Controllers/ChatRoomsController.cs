using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySignalR.Data;
using MySignalR.Models;

namespace MySignalR.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ChatRoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatRoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ChatRooms
        [HttpGet]
        [Route("/[controller]/GetChatRoom")]
        public async Task<ActionResult<IEnumerable<ChatRoom>>> GetChatRoom()
        {
            if (_context.Rooms == null)
            {
                return NotFound();
            }
            return await _context.Rooms.ToListAsync();
        }

        [HttpGet]
        [Route("/[controller]/GetChatUser")]
        public async Task<ActionResult<Object>> GetChatUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var users = await _context.Users.ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            return users.Where(u => u.Id != userId).Select(u => new { u.Id, u.UserName }).ToList();
        }

        // GET: ChatRooms/Create
        public IActionResult Create()
        {
            return View();
        }

        [Route("/[controller]/PostChatRoom")]
        public async Task<ActionResult<ChatRoom>> PostChatRoom(ChatRoom chatRoom)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ChatRoom'  is null.");
            }
            _context.Rooms.Add(chatRoom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatRoom", new { id = chatRoom.Id }, chatRoom);
        }



        // DELETE: api/ChatRooms/5
        [HttpDelete("{id}")]
        [Route("/[controller]/DeleteChatRoom/{id}")]
        public async Task<IActionResult> DeleteChatRoom(int id)
        {
            if (_context.Rooms == null)
            {
                return NotFound();
            }
            var chatRoom = await _context.Rooms.FindAsync(id);
            if (chatRoom == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(chatRoom);
            await _context.SaveChangesAsync();

            var room = await _context.Rooms.FirstOrDefaultAsync();

            return Ok(new { deleted = id, selected = (room == null ? 0 : room.Id) });
        }
    }
}
