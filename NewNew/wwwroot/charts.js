const category = document.getElementById("category");

new Chart(category, {
type: 'bar',
data: {
    labels: ['A.M', 'M.E', 'C.E', 'C.S', 'A.B', 'Arc'],
    datasets: [{
    label: 'Number of books',
    data: [15804, 10342, 13054, 20438, 8392, 12439],
    borderWidth: 1
    }]
},
option: {
    risponsive: true,
    plugins: {
        legend: {
            position: 'left'
        }
    }
}
});

const availability = document.getElementById("availability");

new Chart(availability, {
    type: 'doughnut',
    data: {
        labels: ['Available', 'Reserved'],
        datasets: [{
            label: 'Availability of books in percentage',
            data: [89, 11],
            borderWidth: 1
        }],
    },
    options: {
        risponsive: true,
    }
});

const borrowTrends = document.getElementById("borrow-trends").getContext("2d")

let borrowGradient = borrowTrends.createLinearGradient(0, 0, 0, 400);
borrowGradient.addColorStop(0, "#36a2eb");
borrowGradient.addColorStop(1, "rgba(0, 210, 255, 0.3)");

new Chart(borrowTrends, {
    type: 'line',
    data: {
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        datasets: [{
            label: 'Book Checkout Trend',
            data: [2309, 4738, 4244, 6544, 12656, 2341, 1532, 5643, 7654, 14583, 2121, 1090],
            borderWidth: 1,
            fill: true,
            backgroundColor: borrowGradient,
        }]
    }
});

const returnRates = document.getElementById("return-rates")
new Chart(returnRates, {
    type: 'pie',
    data: {
        labels: ['Returned Books', 'Lost Books'],
        datasets: [{
            label: 'Book Checkout Trend',
            data: [99, 1],
            backgroundColor: ["#36a2eb", "pink"], 
            borderWidth: 1,
        }]
    },
    options: {
        plugins: {
            legend: {
                display: false,
            }
        }
    }
});