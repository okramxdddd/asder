using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tanulokezelo_MVC_API.Models;

namespace Tanulokezelo_MVC_API.Controllers
{
    [Route("api/studentapi")]
    [ApiController]
    public class StudentApiController : ControllerBase
    {
        private static List<Student> tanulok = new List<Student> 
        {
            new Student{Id=1,  OMazonosito=71610001, Nev="Teszt Elek",          EletKor=20, Atlag=4.5},
            new Student{Id=nextId,  OMazonosito=71610002, Nev="a török",             EletKor=19, Atlag=3.8},
            new Student{Id=nextId,  OMazonosito=71610003, Nev="george",              EletKor=21, Atlag=2.9},
            new Student{Id=nextId,  OMazonosito=71610004, Nev="faembör",             EletKor=22, Atlag=4.1},
            new Student{Id=nextId,  OMazonosito=71610005, Nev="hihiha",              EletKor=18, Atlag=3.2},
            new Student{Id=nextId,  OMazonosito=71610006, Nev="eragonist",           EletKor=23, Atlag=4.7},
            new Student{Id=nextId,  OMazonosito=71610007, Nev="evo canon",           EletKor=24, Atlag=2.5},
            new Student{Id=nextId,  OMazonosito=71610008, Nev="Sanyi bacsi",         EletKor=20, Atlag=3.1},
            new Student{Id=nextId,  OMazonosito=71610009, Nev="Kiss Adam AK",        EletKor=19, Atlag=4.0},
            new Student{Id=nextId, OMazonosito=71610010, Nev="shourkou",            EletKor=21, Atlag=3.6},
            new Student{Id=nextId, OMazonosito=71610011, Nev="busty korean mother", EletKor=22, Atlag=2.7},
            new Student{Id=nextId, OMazonosito=71610012, Nev="Ifj Lakatos Dzsordzso romaro romeo adorjan the fourth",           EletKor=20, Atlag=1.2},
            new Student{Id=nextId, OMazonosito=71610013, Nev="Ronaldo the third",   EletKor=25, Atlag=3.4},
            new Student{Id=nextId, OMazonosito=71610014, Nev="Armok",               EletKor=23, Atlag=1.9},
            new Student{Id=nextId, OMazonosito=71610015, Nev="Videki Messi",        EletKor=24, Atlag=4.9},
            new Student{Id=nextId, OMazonosito=71610016, Nev="Bodocs",             EletKor=18, Atlag=3.3},
            new Student{Id=nextId, OMazonosito=71610017, Nev="Ubul",                EletKor=22, Atlag=2.1},
            new Student{Id=nextId, OMazonosito=71610018, Nev="Bongyasbob",                EletKor=22, Atlag=2.6},
            new Student{Id=nextId, OMazonosito=71610019, Nev="csar tamas",                EletKor=23, Atlag=3.2},
            new Student{Id=nextId, OMazonosito=71610020, Nev="fanuc robodrill",                EletKor=12, Atlag=4.1}
        };
        private static int nextId = 2;
        //GET - egész lista
        [HttpGet]
        public IActionResult GetAll()
        {
            //Console.WriteLine(tanulok[0]);
            return Ok(TanuloListaConverter(tanulok));
            //return Ok(tanulok);
        }
        //GET - Id alapján visszadani egy tanulót
        [HttpGet("{om}")]
        public IActionResult GetById(int om)
        {
            var tanulo = tanulok.FirstOrDefault(x => x.OMazonosito == om);

            if (tanulo == null) return NotFound();

            return Ok(new StudentDTO
            {
               OMazonosito=tanulo.OMazonosito,
               Nev=tanulo.Nev,
               EletKor=tanulo.EletKor,
               Atlag=tanulo.Atlag
            });
        }
        //POST - tanuló hozzáad
        [HttpPost("create")]
        public IActionResult AddStudent([FromBody] StudentDTO s)
        {
            //Id beállítása a nextID statikus változóval
            //Taj szam generálása (OM azonosító / életkor)
            if (s == null) return BadRequest();
            Student tanulo = new Student
            {
                Id= nextId++,
                OMazonosito=s.OMazonosito,
                Nev=s.Nev,
                Atlag=s.Atlag,
                EletKor=s.EletKor,
                TAJszam=s.OMazonosito/s.EletKor
            };      
            foreach (var item in tanulok)
            {
                if(item.OMazonosito==tanulo.OMazonosito)
                {
                    tanulo.OMazonosito += 1;
                    tanulok.Add(tanulo);
                }
            }
            
            return Ok(tanulo);
        }
        //PUT - módosítás
        [HttpPut("update/{om}")]
        public IActionResult UpdateStudent(int om, [FromBody] StudentDTO modositott) 
        { 
            if(modositott==null) return BadRequest();

            var tanulo = tanulok.FirstOrDefault(t=>t.OMazonosito==om);

            if (tanulo==null) return NotFound();
            //DTO -> MODEL frissítés
            tanulo.OMazonosito = modositott.OMazonosito;
            tanulo.EletKor = modositott.EletKor;
            tanulo.Atlag=modositott.Atlag;
            tanulo.Nev=modositott.Nev;
            tanulo.TAJszam = modositott.OMazonosito / modositott.EletKor;

            return Ok(tanulo);
        }

        //DELETE - törlés
        [HttpDelete("delete/{om}")]
        public IActionResult DeleteStudent(int om)
        {
            var tanulo = tanulok.FirstOrDefault(t => t.OMazonosito == om);

            if (tanulo == null) return NotFound();

            tanulok.Remove(tanulo);

            //Törlés sikeres, de nincsen visszetérési adat
            return NoContent();
        }

        public List<StudentDTO> TanuloListaConverter(List<Student> lista) 
        {
            List<StudentDTO> tanuloDTOs= new List<StudentDTO>();
            foreach (var tanulo in lista)
            {
                StudentDTO tanuloDTO = new StudentDTO 
                {
                    OMazonosito = tanulo.OMazonosito,
                    Nev=tanulo.Nev,
                    EletKor=tanulo.EletKor,
                    Atlag=tanulo.Atlag
                };
                tanuloDTOs.Add(tanuloDTO);
            }
            return tanuloDTOs;
        }

        
    }
}
