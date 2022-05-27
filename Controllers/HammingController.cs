using HammingCode.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace HammingCode.Controllers
{

    public class HammingController : Controller
    {
        HammingModel h = new HammingModel();
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Coding()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Coding(string symbol,string textboxsymbol)
        {
            int code;
            if (textboxsymbol == null)
            {
                symbol = symbol.Substring(symbol.Length - 2, 1);
                code = (int)symbol[0];
            }
            else code = (int)textboxsymbol[0];
            return RedirectToAction("CodeResult",new{ codeofsymbol = code });
        }

        public IActionResult CodeResult(int codeofsymbol)
        {
            int[] code = new int[1] { codeofsymbol };
            BitArray bitArray = new BitArray(code);
            bool[] checkcodes = new bool[4];
            checkcodes[0] = bitArray[6] ^ bitArray[5] ^ bitArray[3] ^ bitArray[2] ^ bitArray[0];
            checkcodes[1] = bitArray[6] ^ bitArray[4] ^ bitArray[3] ^ bitArray[1] ^ bitArray[0];
            checkcodes[2] = bitArray[5] ^ bitArray[4] ^ bitArray[3];
            checkcodes[3] = bitArray[2] ^ bitArray[1] ^ bitArray[0];
            h.Decode = checkcodes[0].ToString() + checkcodes[1].ToString() + bitArray[6].ToString() + checkcodes[2].ToString() +
                bitArray[5].ToString() + bitArray[4].ToString() + bitArray[3].ToString() + checkcodes[3].ToString() +
                bitArray[2].ToString() + bitArray[1].ToString() + bitArray[0].ToString();
            h.Decode=h.Decode.Replace("True", "1");
            h.Decode=h.Decode.Replace("False", "0");
            h.Symbol = ((char)codeofsymbol).ToString();
            return View(h);
        }
        [HttpGet]
        [Route("Hamming/Decoding/{stringcode?}")]
        public IActionResult Decoding(string stringcode)
        {
            if(String.IsNullOrEmpty(stringcode))
            {
                ViewBag.Code = "";
            }
            ViewBag.Code = stringcode;
            return View();
        }

        [HttpPost]
        public IActionResult Decoding(HammingModel code)
        {
            if (String.IsNullOrEmpty(code.Decode))
            {
                return View("");
            }
            return RedirectToAction("DecodingResult",new {codefordecode = code.Decode});
        }

        public IActionResult DecodingResult(string codefordecode)
        {
            h.Decode = codefordecode;
            bool[] bits = new bool[11];
            for(int i=0;i< codefordecode.Length; i++)
            {
                bits[i] = codefordecode[i].ToString() == "1" ? true : false;
            }
            BitArray bitArray = new BitArray(bits);
            bool[] checkcodes = new bool[4];
            checkcodes[0] = bitArray[2] ^ bitArray[4] ^ bitArray[6] ^ bitArray[8] ^ bitArray[10];
            checkcodes[1] = bitArray[2] ^ bitArray[5] ^ bitArray[6] ^ bitArray[9] ^ bitArray[10];
            checkcodes[2] = bitArray[4] ^ bitArray[5] ^ bitArray[6];
            checkcodes[3] = bitArray[8] ^ bitArray[9] ^ bitArray[10];
            bool[] controlbits = new bool[4];
            controlbits[0] = bitArray[7] ^ checkcodes[3];
            controlbits[1] = bitArray[3] ^ checkcodes[2];
            controlbits[2] = bitArray[1] ^ checkcodes[1];
            controlbits[3] = bitArray[0] ^ checkcodes[0];
            string checkcode = String.Join("", controlbits);
            checkcode = checkcode.Replace("True", "1").Replace("False", "0");
            ViewBag.Control = checkcode;
            ViewBag.Wrong = Convert.ToInt32(checkcode, 2);
            bitArray[ViewBag.Wrong-1] = !bitArray[ViewBag.Wrong - 1];
            string symbolcode = bitArray[2].ToString() + bitArray[4].ToString() + bitArray[5].ToString() + bitArray[6].ToString() +
                bitArray[8].ToString() + bitArray[9].ToString() + bitArray[10].ToString();
            symbolcode = symbolcode.Replace("True", "1").Replace("False", "0");
            h.Symbol = ((char)Convert.ToInt32(symbolcode,2)).ToString();
            bool[] arr = new bool[11];
            bitArray.CopyTo(arr, 0);
            ViewBag.CorrectCode = String.Join("", arr).Replace("True", "1").Replace("False", "0");
            return View(h);
        }
    }
}
