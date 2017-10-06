namespace ProductsZulu.API.Controllers
{
    using ProductsZulu.API.Models;
    using ProductsZuluDomain;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    public class CategoriesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Categories
        public async Task<IHttpActionResult> GetCategories()
        {
            //Este metodo arma una lista de productos dentro del objeto category
            //El objeto category y products fueron modificados en el API para poder
            //Recibir la lista armada
            var categories = await db.Categories.ToArrayAsync();
            var categoriesResponse = new List<CategoryResponse>();

            foreach (var category in categories)
            {
                var productsResponse = new List<ProductResponse>();
                foreach (var product in category.Products)
                {
                    productsResponse.Add(new ProductResponse
                    {
                        Description = product.Description,
                        Image = product.Image,
                        IsActtive = product.IsActtive,
                        LastPurchase = product.LastPurchase,
                        Price = product.Price,
                        ProductId = product.ProductId,
                        Remarks = product.Remarks,
                        Stock = product.Stock
                    });
                }
                categoriesResponse.Add(new CategoryResponse
                {
                    CategoryId = category.CategoryId,
                    Description = category.Description,
                    Products = productsResponse,
                });
            }

            return Ok(categoriesResponse); //Ok => deserializa a un objeto Json
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Index"))
                {
                    return BadRequest("There are a record with the same description.");
                }
                else
                {
                    BadRequest(ex.Message);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Index"))
                {
                    return BadRequest("There are a record with the same description.");
                }
                else
                {
                    BadRequest(ex.Message);
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> DeleteCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    return BadRequest("You can't delete this record because it has relate records.");
                }
                else
                {
                    BadRequest(ex.Message);
                }
            }

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryId == id) > 0;
        }
    }
}