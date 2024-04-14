using LOCALink.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOCALink.Controllers
{
    public class BaseController : Controller
    {
        public LOCALinkEntities _db;
        public BaseRepository<User_Account> _userRepo;
        public BaseRepository<Booking> _booking;
        public BaseController()
        {
            _db = new LOCALinkEntities();
            _userRepo = new BaseRepository<User_Account>();
            _booking = new BaseRepository<Booking>();

        }
    }
}