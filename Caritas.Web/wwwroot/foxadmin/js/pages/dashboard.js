//[Dashboard Javascript]

//Project:	Fox Admin - Responsive Admin Template
//Primary use:   Used only for the main dashboard (index.html)


$(function () {

  'use strict';
	
	
	$('.countnm').each(function () {
		$(this).prop('Counter',0).animate({
			Counter: $(this).text()
		}, {
			duration: 5000,
			easing: 'swing',
			step: function (now) {
				$(this).text(Math.ceil(now));
			}
		});
	});
	
	
	var options = {
	chart: {
		height: 350,
		type: 'line',
		shadow: {
			enabled: false,
			color: '#bbb',
			top: 3,
			left: 2,
			blur: 3,
			opacity: 1
		},
	},
	stroke: {
		width: 7,   
		curve: 'smooth'
	},
	series: [{
		name: 'Likes',
		data: [4, 3, 10, 9, 29, 19, 22, 9, 12, 7, 19, 5, 13, 9, 17, 2, 7, 5]
	}],
	xaxis: {
		type: 'datetime',
		categories: ['1/11/2000', '2/11/2000', '3/11/2000', '4/11/2000', '5/11/2000', '6/11/2000', '7/11/2000', '8/11/2000', '9/11/2000', '10/11/2000', '11/11/2000', '12/11/2000', '1/11/2001', '2/11/2001', '3/11/2001','4/11/2001' ,'5/11/2001' ,'6/11/2001'],
	},
	fill: {
		type: 'gradient',
		gradient: {
			shade: 'dark',
			gradientToColors: [ '#45AEF1'],
			shadeIntensity: 1,
			type: 'horizontal',
			opacityFrom: 1,
			opacityTo: 1,
			stops: [0, 100, 100, 100]
		},
	},
	markers: {
		size: 4,
		opacity: 0.9,
		colors: ["#45AEF1"],
		strokeColor: "#fff",
		strokeWidth: 2,

		hover: {
			size: 7,
		}
	},
	yaxis: {
		min: 0,
		max: 40,                
	}
}

var chart = new ApexCharts(
	document.querySelector("#chart-line2"),
	options
);

chart.render();


var options = {
  chart: {
	height: 350,
	type: 'line',
	stacked: false,
	  zoom: {
		enabled: false,
	  }
  },
  stroke: {
	width: [0, 2, 5],
	curve: 'smooth'
  },
  plotOptions: {
	bar: {
	  columnWidth: '50%'
	}
  },
  colors: ['#45AEF1', '#FFB22B', '#E03656'],
  series: [{
	name: 'Facebook',
	type: 'column',
	data: [23, 11, 22, 27, 13, 22, 37, 21, 44, 22, 30]
  }, {
	name: 'Vine',
	type: 'area',
	data: [44, 55, 41, 67, 22, 43, 21, 41, 56, 27, 43]
  }, {
	name: 'Dribbble',
	type: 'line',
	data: [30, 25, 36, 30, 45, 35, 64, 52, 59, 36, 39]
  }],
  fill: {
	opacity: [0.85,0.25,1],
		gradient: {
			inverseColors: false,
			shade: 'light',
			type: "vertical",
			opacityFrom: 0.85,
			opacityTo: 0.55,
			stops: [0, 100, 100, 100]
		}
  },
  labels: ['01/01/2003', '02/01/2003','03/01/2003','04/01/2003','05/01/2003','06/01/2003','07/01/2003','08/01/2003','09/01/2003','10/01/2003','11/01/2003'],
  markers: {
	size: 0
  },
  xaxis: {
	type:'datetime'
  },
  yaxis: {
	min: 0
  },
  tooltip: {
	shared: true,
	intersect: false,
	y: {
	  formatter: function (y) {
		if(typeof y !== "undefined") {
		  return  y.toFixed(0) + " views";
		}
		return y;

	  }
	}
  },

  legend: {				
	  position: 'top',
	  horizontalAlign: 'left',
	labels: {
	  useSeriesColors: true,	
	},
  }
}

var chart = new ApexCharts(
  document.querySelector("#chart-line3"),
  options
);

chart.render();
	
	
	
	var options = {
          series: [44, 55, 41, 17],
			labels: ['Social', 'Email', 'New Join', 'Active'],
          chart: {
			  width: 250,
			  type: 'donut',
			},
		dataLabels: {
    		enabled: false,
		},
		  plotOptions: {
			pie: {
			  donut: {
				size: '80%',
				  labels: {
				  show: true,
				  name: {
				  },
				  value: {
				  }
				}
			  },
			}
		  },
		colors: ['#45AEF1', '#7460EE', '#26C6DA', '#FFB22B'],
        responsive: [{
          breakpoint: 480,
          options: {
            chart: {
              width: 200
            },
            legend: {
              show: false
            }
          }
        }],
		
            legend: {
              show: false
            }
        };

        var chart = new ApexCharts(document.querySelector("#charts_widget_3_chart"), options);
        chart.render();
	
	
	var options = {
		  chart: {
			height: 190,
			type: "radialBar"
		  },

		  series: [67],
			colors: ["#E03656"],
		  plotOptions: {
			radialBar: {
			  hollow: {
				margin: 15,
				size: "70%"
			  },

			  dataLabels: {
				showOn: "always",
				name: {
				  offsetY: -10,
				  show: false,
				  color: "#888",
				  fontSize: "13px"
				},
				value: {
				  color: "#111",
				  fontSize: "30px",
				  show: true
				}
			  }
			}
		  },

		  stroke: {
			lineCap: "round",
		  },
		  labels: ["Progress"]
		};

		var chart = new ApexCharts(document.querySelector("#revenue5"), options);

		chart.render();
	
	
}); // End of use strict
