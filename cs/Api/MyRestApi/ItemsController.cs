using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private static List<Item> Items = new List<Item>
    {
        new Item { Id = 1, Name = "Item1", Price = 10.99M },
        new Item { Id = 2, Name = "Item2", Price = 20.99M }
    };

    // GET: api/items
    [HttpGet]
    public ActionResult<IEnumerable<Item>> GetAll()
    {
        return Ok(Items);
    }

    // GET: api/items/{id}
    [HttpGet("{id}")]
    public ActionResult<Item> GetById(int id)
    {
        var item = Items.FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    // POST: api/items
    [HttpPost]
    public ActionResult<Item> Create(Item newItem)
    {
        newItem.Id = Items.Count + 1;
        Items.Add(newItem);
        return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
    }

    // PUT: api/items/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, Item updatedItem)
    {
        var item = Items.FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        item.Name = updatedItem.Name;
        item.Price = updatedItem.Price;
        return NoContent();
    }

    // DELETE: api/items/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = Items.FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        Items.Remove(item);
        return NoContent();
    }
}
