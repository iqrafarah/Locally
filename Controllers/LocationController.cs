using Locally.Models;
using Locally.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Locally.Controllers;

public class LocationController : Controller
{
    private readonly ILocationRepository _locationRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public LocationController(
        ILocationRepository locationRepository,
        IWebHostEnvironment webHostEnvironment)
    {
        _locationRepository = locationRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        string userId = "demo-user";
        List<Location> locations = _locationRepository.GetByUser(userId);

        return View(locations);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Location location, List<IFormFile> images)
    {
        if (!ModelState.IsValid)
        {
            return View(location);
        }

        location.UserId = "demo-user";
        location.Status = LocationStatus.Pending;

        if (images != null && images.Count > 0)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (IFormFile image in images)
            {
                if (image.Length > 0)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using FileStream stream = new FileStream(filePath, FileMode.Create);
                    image.CopyTo(stream);

                    location.ImagePaths.Add("/uploads/" + fileName);
                }
            }
        }

        _locationRepository.Add(location);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        Location? location = _locationRepository.GetById(id);

        if (location == null)
        {
            return NotFound();
        }

        return View(location);
    }

    [HttpPost]
    public IActionResult Edit(Location location)
    {
        if (!ModelState.IsValid)
        {
            return View(location);
        }

        _locationRepository.Update(location);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        _locationRepository.Delete(id);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error()
    {
        return View("~/Views/Shared/Error.cshtml");
    }
}