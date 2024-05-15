using NashTechListPersonMVC.BusinessLogic.Interfaces;
using NashTechListPersonMVC.WebApp.Areas.NashTech.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NashTechListPersonMVC.BusinessLogic.Interfaces;
using NashTechListPersonMVC.BusinessLogic.ViewModels;
using NashTechListPersonMVC.Model.Models;
using NashTechListPersonMVC.WebApp.Areas.NashTech.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NashTechListPersonMVC.Test.ControllerTests
{
    public class RookiesControllerTests
    {
        private readonly Mock<IPersonBusinessLogic> _mockPersonBusinessLogic;
        private readonly RookiesController _controller;

        public RookiesControllerTests()
        {
            _mockPersonBusinessLogic = new Mock<IPersonBusinessLogic>();
            _controller = new RookiesController(_mockPersonBusinessLogic.Object);
        }

    }
