using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;
using System.Collections;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoBuilderController : ControllerBase
    {
        private readonly IAutoBuildingService _builderService;
        private readonly IDatabaseOperationsService _databaseOperationsService;

        public AutoBuilderController(IAutoBuildingService builderService, IDatabaseOperationsService databaseOperationsService)
        {
            _builderService = builderService;
            _databaseOperationsService = databaseOperationsService;
        }

        // returns cart contents
        // GET: api/<CartController>
        [HttpGet]
        public Robot GetRobot()
        {
            return _builderService.GetRobot();
        }

        // used to add new product to cart
        // POST api/<CartController>/add
        [HttpPost]
        public async Task GenerationParameters([FromBody] AutoGenerator value)
        {
            Robot robot = new Robot();
            List<Product> _products = await _databaseOperationsService.ReadListAsync<Product>($"SELECT * FROM preke");
            List<Product> _productsOneParameter = new List<Product>();
            List<Product> _productsSecondParameter = new List<Product>();
            List<Product> _productsThirdParameter = new List<Product>();
            Hashtable hashtable = new Hashtable();
            List<Product> leftLeg = new List<Product>();
            List<Product> rightLeg = new List<Product>();
            List<Product> body = new List<Product>();
            List<Product> rightArm = new List<Product>();
            List<Product> leftArm = new List<Product>();
            List<Product> head = new List<Product>();
            Random random = new Random();
            foreach (Product product in _products)
            {
                switch (product.Category)
                {
                    case 2:
                        rightLeg.Add(product);
                        break;
                    case 3:
                        head.Add(product);
                        break;
                    case 4:
                        leftArm.Add(product);
                        break;
                    case 5:
                        rightArm.Add(product);
                        break;
                    case 6:
                        leftLeg.Add(product);
                        break;
                    case 7:
                        body.Add(product);
                        break;
                }
            }
            if (rightLeg.Count == 0 || head.Count == 0 || leftArm.Count == 0 || rightArm.Count == 0 || leftLeg.Count == 0 || body.Count == 0) { Console.WriteLine("Doomed"); }
            else
            {
                hashtable.Add(2, rightLeg);
                hashtable.Add(3, head);
                hashtable.Add(4, leftArm);
                hashtable.Add(5, rightArm);
                hashtable.Add(6, leftLeg);
                hashtable.Add(7, body);
            }
            var tempRightLeg = (List<Product>)hashtable[2];
            var tempHead = (List<Product>)hashtable[3];
            var tempLeftArm = (List<Product>)hashtable[4];
            var tempRightArm = (List<Product>)hashtable[5];
            var tempLeftLeg = (List<Product>)hashtable[6];
            var tempBody = (List<Product>)hashtable[7];
            //robot.RightLeg = tempRightLeg[0].Id;
            //robot.Head = tempHead[0].Id;
            //robot.LeftArm = tempLeftArm[0].Id;
            //robot.RightArm = tempRightArm[0].Id;
            //robot.LeftLeg = tempLeftLeg[0].Id;
            //robot.Body = tempBody[0].Id;
            switch (value.Zodiac)
            {
                case 1:
                case 5:
                    if (value.Fighting_Style == 1)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Attack >= 7);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Attack >= 7);
                        tempHead = tempHead.FindAll(x => x.Attack >= 7);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Attack >= 7);
                        tempRightArm = tempRightArm.FindAll(x => x.Attack >= 7);
                        tempBody = tempBody.FindAll(x => x.Attack >= 7);

                    }
                    if (value.Fighting_Style == 2)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        tempHead = tempHead.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        tempRightArm = tempRightArm.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        tempBody = tempBody.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                    }
                    if (value.Fighting_Style == 3)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Defense >= 7);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Defense >= 7);
                        tempHead = tempHead.FindAll(x => x.Defense >= 7);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Defense >= 7);
                        tempRightArm = tempRightArm.FindAll(x => x.Defense >= 7);
                        tempBody = tempBody.FindAll(x => x.Defense >= 7);
                    }

                    if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0 && tempBody.Count != 0)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);

                        if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0 && tempBody.Count != 0)
                        {
                            var index = random.Next(tempBody.Count);
                            robot.Body = tempBody[index].Id;
                            var connection = tempBody[index].Connection;
                            tempRightLeg = tempRightLeg.FindAll(x => x.Connection.Equals(connection));
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Connection.Equals(connection));
                            tempHead = tempHead.FindAll(x => x.Connection.Equals(connection));
                            tempLeftArm = tempLeftArm.FindAll(x => x.Connection.Equals(connection));
                            tempRightArm = tempRightArm.FindAll(x => x.Connection.Equals(connection));

                            if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0)
                            {
                                robot.Head = tempHead[random.Next(tempHead.Count)].Id;
                                robot.LeftArm = tempLeftArm[random.Next(tempLeftArm.Count)].Id;
                                robot.RightArm = tempRightArm[random.Next(tempRightArm.Count)].Id;
                                robot.RightLeg = tempRightLeg[random.Next(tempRightLeg.Count)].Id;
                                robot.LeftLeg = tempLeftLeg[random.Next(tempLeftLeg.Count)].Id;
                            }
                            else
                            {
                                robot.Nickname = "Unable";
                            }
                        }
                        else
                        {
                            robot.Nickname = "Unable";
                        }
                    }
                    else
                    {
                        robot.Nickname = "Unable";
                    }
                    break;

                case 2:
                case 8:
                    if (value.Material == 1)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material);
                    }
                    if (value.Material == 2)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material);
                    }
                    if (value.Material == 4)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material);
                    }
                    if (value.Material == 5)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material);
                    }
                    if (value.Material == 6)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material);
                    }
                    if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0 && tempBody.Count != 0)
                    {
                        if (value.Budget == 1)
                        {

                            tempRightLeg = tempRightLeg.FindAll(x => x.Price < 250);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Price < 250);
                            tempHead = tempHead.FindAll(x => x.Price < 250);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Price < 250);
                            tempRightArm = tempRightArm.FindAll(x => x.Price < 250);
                            tempBody = tempBody.FindAll(x => x.Price < 250);
                        }
                        if (value.Budget == 2)
                        {

                            tempRightLeg = tempRightLeg.FindAll(x => x.Price > 250 && x.Price < 500);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Price > 250 && x.Price < 500);
                            tempHead = tempHead.FindAll(x => x.Price > 250 && x.Price < 500);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Price > 250 && x.Price < 500);
                            tempRightArm = tempRightArm.FindAll(x => x.Price > 250 && x.Price < 500);
                            tempBody = tempBody.FindAll(x => x.Price > 250 && x.Price < 500);
                        }
                        if (value.Budget == 3)
                        {
                            tempRightLeg = tempRightLeg.FindAll(x => x.Price >= 500);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Price >= 500);
                            tempHead = tempHead.FindAll(x => x.Price >= 500);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Price >= 500);
                            tempRightArm = tempRightArm.FindAll(x => x.Price >= 500);
                            tempBody = tempBody.FindAll(x => x.Price >= 500);
                        }
                        if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0 && tempBody.Count != 0)
                        {
                            var index = random.Next(tempBody.Count);
                            robot.Body = tempBody[index].Id;
                            var connection = tempBody[index].Connection;
                            tempRightLeg = tempRightLeg.FindAll(x => x.Connection.Equals(connection));
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Connection.Equals(connection));
                            tempHead = tempHead.FindAll(x => x.Connection.Equals(connection));
                            tempLeftArm = tempLeftArm.FindAll(x => x.Connection.Equals(connection));
                            tempRightArm = tempRightArm.FindAll(x => x.Connection.Equals(connection));

                            if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0)
                            {
                                robot.Head = tempHead[random.Next(tempHead.Count)].Id;
                                robot.LeftArm = tempLeftArm[random.Next(tempLeftArm.Count)].Id;
                                robot.RightArm = tempRightArm[random.Next(tempRightArm.Count)].Id;
                                robot.RightLeg = tempRightLeg[random.Next(tempRightLeg.Count)].Id;
                                robot.LeftLeg = tempLeftLeg[random.Next(tempLeftLeg.Count)].Id;
                            }
                            else
                            {
                                robot.Nickname = "Unable";
                            }
                        }
                        else
                        {
                            robot.Nickname = "Unable";
                        }
                    }
                    else
                    {
                        robot.Nickname = "Unable";
                    }
                    break;

                case 3:
                case 11:
                    if (value.Budget == 1)
                    {

                        tempRightLeg = tempRightLeg.FindAll(x => x.Price < 250);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Price < 250);
                        tempHead = tempHead.FindAll(x => x.Price < 250);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Price < 250);
                        tempRightArm = tempRightArm.FindAll(x => x.Price < 250);
                        tempBody = tempBody.FindAll(x => x.Price < 250);
                    }
                    if (value.Budget == 2)
                    {

                        tempRightLeg = tempRightLeg.FindAll(x => x.Price > 250 && x.Price < 500);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Price > 250 && x.Price < 500);
                        tempHead = tempHead.FindAll(x => x.Price > 250 && x.Price < 500);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Price > 250 && x.Price < 500);
                        tempRightArm = tempRightArm.FindAll(x => x.Price > 250 && x.Price < 500);
                        tempBody = tempBody.FindAll(x => x.Price > 250 && x.Price < 500);
                    }
                    if (value.Budget == 3)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Price >= 500);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Price >= 500);
                        tempHead = tempHead.FindAll(x => x.Price >= 500);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Price >= 500);
                        tempRightArm = tempRightArm.FindAll(x => x.Price >= 500);
                        tempBody = tempBody.FindAll(x => x.Price >= 500);
                    }
                    if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0 && tempBody.Count != 0)
                    {
                        if (value.Fighting_Style == 1)
                        {
                            tempRightLeg = tempRightLeg.FindAll(x => x.Attack >= 7);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Attack >= 7);
                            tempHead = tempHead.FindAll(x => x.Attack >= 7);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Attack >= 7);
                            tempRightArm = tempRightArm.FindAll(x => x.Attack >= 7);
                            tempBody = tempBody.FindAll(x => x.Attack >= 7);

                        }
                        if (value.Fighting_Style == 2)
                        {
                            tempRightLeg = tempRightLeg.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                            tempHead = tempHead.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                            tempRightArm = tempRightArm.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                            tempBody = tempBody.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        }
                        if (value.Fighting_Style == 3)
                        {
                            tempRightLeg = tempRightLeg.FindAll(x => x.Defense >= 7);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Defense >= 7);
                            tempHead = tempHead.FindAll(x => x.Defense >= 7);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Defense >= 7);
                            tempRightArm = tempRightArm.FindAll(x => x.Defense >= 7);
                            tempBody = tempBody.FindAll(x => x.Defense >= 7);
                        }
                        var index = random.Next(tempBody.Count);
                        robot.Body = tempBody[index].Id;
                        var connection = tempBody[index].Connection;
                        tempRightLeg = tempRightLeg.FindAll(x => x.Connection.Equals(connection));
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Connection.Equals(connection));
                        tempHead = tempHead.FindAll(x => x.Connection.Equals(connection));
                        tempLeftArm = tempLeftArm.FindAll(x => x.Connection.Equals(connection));
                        tempRightArm = tempRightArm.FindAll(x => x.Connection.Equals(connection));

                        if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0)
                        {
                            robot.Head = tempHead[random.Next(tempHead.Count)].Id;
                            robot.LeftArm = tempLeftArm[random.Next(tempLeftArm.Count)].Id;
                            robot.RightArm = tempRightArm[random.Next(tempRightArm.Count)].Id;
                            robot.RightLeg = tempRightLeg[random.Next(tempRightLeg.Count)].Id;
                            robot.LeftLeg = tempLeftLeg[random.Next(tempLeftLeg.Count)].Id;
                        }
                        else
                        {
                            robot.Nickname = "Unable";
                        }
                    }
                    else
                    {
                        robot.Nickname = "Unable";
                    }
                    break;
                case 4:
                case 9:
                    if (value.Fighting_Style == 1)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Attack >= 7);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Attack >= 7);
                        tempHead = tempHead.FindAll(x => x.Attack >= 7);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Attack >= 7);
                        tempRightArm = tempRightArm.FindAll(x => x.Attack >= 7);
                        tempBody = tempBody.FindAll(x => x.Attack >= 7);

                    }
                    if (value.Fighting_Style == 2)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        tempHead = tempHead.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        tempRightArm = tempRightArm.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        tempBody = tempBody.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                    }
                    if (value.Fighting_Style == 3)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Defense >= 7);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Defense >= 7);
                        tempHead = tempHead.FindAll(x => x.Defense >= 7);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Defense >= 7);
                        tempRightArm = tempRightArm.FindAll(x => x.Defense >= 7);
                        tempBody = tempBody.FindAll(x => x.Defense >= 7);
                    }


                    if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0 && tempBody.Count != 0)
                    {
                        if (value.Budget == 1)
                        {

                            tempRightLeg = tempRightLeg.FindAll(x => x.Price < 250);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Price < 250);
                            tempHead = tempHead.FindAll(x => x.Price < 250);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Price < 250);
                            tempRightArm = tempRightArm.FindAll(x => x.Price < 250);
                            tempBody = tempBody.FindAll(x => x.Price < 250);
                        }
                        if (value.Budget == 2)
                        {

                            tempRightLeg = tempRightLeg.FindAll(x => x.Price > 250 && x.Price < 500);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Price > 250 && x.Price < 500);
                            tempHead = tempHead.FindAll(x => x.Price > 250 && x.Price < 500);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Price > 250 && x.Price < 500);
                            tempRightArm = tempRightArm.FindAll(x => x.Price > 250 && x.Price < 500);
                            tempBody = tempBody.FindAll(x => x.Price > 250 && x.Price < 500);
                        }
                        if (value.Budget == 3)
                        {
                            tempRightLeg = tempRightLeg.FindAll(x => x.Price >= 500);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Price >= 500);
                            tempHead = tempHead.FindAll(x => x.Price >= 500);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Price >= 500);
                            tempRightArm = tempRightArm.FindAll(x => x.Price >= 500);
                            tempBody = tempBody.FindAll(x => x.Price >= 500);
                        }
                        if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0 && tempBody.Count != 0)
                        {
                            var index = random.Next(tempBody.Count);
                            robot.Body = tempBody[index].Id;
                            var connection = tempBody[index].Connection;
                            tempRightLeg = tempRightLeg.FindAll(x => x.Connection.Equals(connection));
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Connection.Equals(connection));
                            tempHead = tempHead.FindAll(x => x.Connection.Equals(connection));
                            tempLeftArm = tempLeftArm.FindAll(x => x.Connection.Equals(connection));
                            tempRightArm = tempRightArm.FindAll(x => x.Connection.Equals(connection));

                            if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0)
                            {
                                robot.Head = tempHead[random.Next(tempHead.Count)].Id;
                                robot.LeftArm = tempLeftArm[random.Next(tempLeftArm.Count)].Id;
                                robot.RightArm = tempRightArm[random.Next(tempRightArm.Count)].Id;
                                robot.RightLeg = tempRightLeg[random.Next(tempRightLeg.Count)].Id;
                                robot.LeftLeg = tempLeftLeg[random.Next(tempLeftLeg.Count)].Id;
                            }
                            else
                            {
                                robot.Nickname = "Unable";
                            }
                        }
                        else
                        {
                            robot.Nickname = "Unable";
                        }
                    }
                    else
                    {
                        robot.Nickname = "Unable";
                    }
                    break;
                case 6:
                case 7:
                    if (value.Budget == 1)
                    {

                        tempRightLeg = tempRightLeg.FindAll(x => x.Price < 250);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Price < 250);
                        tempHead = tempHead.FindAll(x => x.Price < 250);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Price < 250);
                        tempRightArm = tempRightArm.FindAll(x => x.Price < 250);
                        tempBody = tempBody.FindAll(x => x.Price < 250);
                    }
                    if (value.Budget == 2)
                    {

                        tempRightLeg = tempRightLeg.FindAll(x => x.Price > 250 && x.Price < 500);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Price > 250 && x.Price < 500);
                        tempHead = tempHead.FindAll(x => x.Price > 250 && x.Price < 500);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Price > 250 && x.Price < 500);
                        tempRightArm = tempRightArm.FindAll(x => x.Price > 250 && x.Price < 500);
                        tempBody = tempBody.FindAll(x => x.Price > 250 && x.Price < 500);
                    }
                    if (value.Budget == 3)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Price >= 500);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Price >= 500);
                        tempHead = tempHead.FindAll(x => x.Price >= 500);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Price >= 500);
                        tempRightArm = tempRightArm.FindAll(x => x.Price >= 500);
                        tempBody = tempBody.FindAll(x => x.Price >= 500);
                    }
                    if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0 && tempBody.Count != 0)
                    {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material || x.Material == value.Material + 1 || x.Material == value.Material - 1);

                        if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0 && tempBody.Count != 0)
                        {
                            var index = random.Next(tempBody.Count);
                            robot.Body = tempBody[index].Id;
                            var connection = tempBody[index].Connection;
                            tempRightLeg = tempRightLeg.FindAll(x => x.Connection.Equals(connection));
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Connection.Equals(connection));
                            tempHead = tempHead.FindAll(x => x.Connection.Equals(connection));
                            tempLeftArm = tempLeftArm.FindAll(x => x.Connection.Equals(connection));
                            tempRightArm = tempRightArm.FindAll(x => x.Connection.Equals(connection));

                            if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0)
                            {
                                robot.Head = tempHead[random.Next(tempHead.Count)].Id;
                                robot.LeftArm = tempLeftArm[random.Next(tempLeftArm.Count)].Id;
                                robot.RightArm = tempRightArm[random.Next(tempRightArm.Count)].Id;
                                robot.RightLeg = tempRightLeg[random.Next(tempRightLeg.Count)].Id;
                                robot.LeftLeg = tempLeftLeg[random.Next(tempLeftLeg.Count)].Id;
                            }
                            else
                            {
                                robot.Nickname = "Unable";
                            }
                        }
                        else
                        {
                            robot.Nickname = "Unable";
                        }
                    }
                    else
                    {
                        robot.Nickname = "Unable";
                    }
                    break;

                case 10:
                case 12:
                    if (value.Material == 1) {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material);
                    }
                    if (value.Material == 2) {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material);
                    }
                    if (value.Material == 4) {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material);
                    }
                    if (value.Material == 5) {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material);
                    }
                    if (value.Material == 6) {
                        tempRightLeg = tempRightLeg.FindAll(x => x.Material == value.Material);
                        tempLeftLeg = tempLeftLeg.FindAll(x => x.Material == value.Material);
                        tempHead = tempHead.FindAll(x => x.Material == value.Material);
                        tempLeftArm = tempLeftArm.FindAll(x => x.Material == value.Material);
                        tempRightArm = tempRightArm.FindAll(x => x.Material == value.Material);
                        tempBody = tempBody.FindAll(x => x.Material == value.Material);
                    }
                    if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0 && tempBody.Count != 0)
                    {
                        if (value.Fighting_Style == 1)
                        {
                            tempRightLeg = tempRightLeg.FindAll(x => x.Attack >= 5);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Attack >= 5);
                            tempHead = tempHead.FindAll(x => x.Attack >= 5);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Attack >= 5);
                            tempRightArm = tempRightArm.FindAll(x => x.Attack >= 5);
                            tempBody = tempBody.FindAll(x => x.Attack >= 5);

                        }
                        if (value.Fighting_Style == 2)
                        {
                            tempRightLeg = tempRightLeg.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                            tempHead = tempHead.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                            tempRightArm = tempRightArm.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                            tempBody = tempBody.FindAll(x => x.Defense == x.Attack || x.Defense == x.Attack - 1 || x.Defense == x.Attack + 1);
                        }
                        if (value.Fighting_Style == 3)
                        {
                            tempRightLeg = tempRightLeg.FindAll(x => x.Defense >= 5);
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Defense >= 5);
                            tempHead = tempHead.FindAll(x => x.Defense >= 5);
                            tempLeftArm = tempLeftArm.FindAll(x => x.Defense >= 5);
                            tempRightArm = tempRightArm.FindAll(x => x.Defense >= 5);
                            tempBody = tempBody.FindAll(x => x.Defense >= 5);
                        }
                        if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0 && tempBody.Count != 0)
                        {
                            var index = random.Next(tempBody.Count);
                            robot.Body = tempBody[index].Id;
                            var connection = tempBody[index].Connection;
                            tempRightLeg = tempRightLeg.FindAll(x => x.Connection.Equals(connection));
                            tempLeftLeg = tempLeftLeg.FindAll(x => x.Connection.Equals(connection));
                            tempHead = tempHead.FindAll(x => x.Connection.Equals(connection));
                            tempLeftArm = tempLeftArm.FindAll(x => x.Connection.Equals(connection));
                            tempRightArm = tempRightArm.FindAll(x => x.Connection.Equals(connection));

                            if (tempRightLeg.Count != 0 && tempHead.Count != 0 && tempLeftArm.Count != 0 && tempRightArm.Count != 0 && tempLeftLeg.Count != 0)
                            {
                                robot.Head = tempHead[random.Next(tempHead.Count)].Id;
                                robot.LeftArm = tempLeftArm[random.Next(tempLeftArm.Count)].Id;
                                robot.RightArm = tempRightArm[random.Next(tempRightArm.Count)].Id;
                                robot.RightLeg = tempRightLeg[random.Next(tempRightLeg.Count)].Id;
                                robot.LeftLeg = tempLeftLeg[random.Next(tempLeftLeg.Count)].Id;
                            }
                            else
                            {
                                robot.Nickname = "Unable";
                            }
                        }
                        else
                        {
                            robot.Nickname = "Unable";
                        }
                    }
                    else
                    {
                        robot.Nickname = "Unable";
                    }
                    break;
            }
            _builderService.GenerateRobot(robot);
            await Task.CompletedTask;
        }
    }
}
