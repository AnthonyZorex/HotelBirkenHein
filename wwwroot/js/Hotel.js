document.addEventListener('DOMContentLoaded', function () {

    const formhotel = document.getElementById('hotelForm');
    const alertBox = document.getElementById('HotelAlert');

    const formzimmer = document.getElementById('ZimmerForm');
    const alertBoxZimmer = document.getElementById('ZimmerAlert');


    let hotelSelect = document.getElementById("hotelSelect");

  
    if (hotelSelect != null) {
        if (hotelSelect.value != "") {
            if (hotelSelect.value != "Wählen Sie das Hotel aus") {
                HotelSelect(hotelSelect.value);
            }
        }
    }

    
    function showAlert(message, type) {
        alertBox.textContent = message;
        alertBox.className = `alert alert-${type} mt-3`;
        alertBox.classList.remove('d-none');
    }

    // 📌 Обработка формы Zimmer
    if (formzimmer) {
        formzimmer.addEventListener('submit', function (e)
        {
            e.preventDefault();
            const formDataZimmer = new FormData(formzimmer);
            const isValidZimmer = formzimmer.checkValidity();

            let zimmerSelect = document.getElementById("zimmerSelect");
            let zimmerSelectx = document.getElementById("zimmerSelectx");

            if (isValidZimmer) {
                axios.post('/Edit/Zimmer', formDataZimmer, {
                    headers: { 'Content-Type': 'multipart/form-data' }
                })
               
                    .then(response => {
                        showAlert(response.data.message || 'Успешно отправлено!', 'success');

                        let option = document.createElement("option");
                        let lastItem = response.data[response.data.length - 1];
                        option.value = lastItem.zimmernummer;
                        option.textContent = lastItem.zimmernummer;

                        zimmerSelect.appendChild(option);
                        zimmerSelectx.appendChild(option);

                    })
                    .catch(error => {
                        const errorMsg = error.response?.data?.message || error.message || 'Произошла ошибка';
                        showAlert(errorMsg, 'danger');
                    });
            } else {
                formzimmer.classList.add('was-validated');
            }
        });
    }

    // 📌 Обработка формы Hotel
    if (formhotel) {
        formhotel.addEventListener('submit', function (e) {
            e.preventDefault();
            const formDataHotel = new FormData(formhotel);
            const isValidHotel = formhotel.checkValidity();

            if (isValidHotel)
            {
                let hotel = document.getElementById("hotelSelect");
                let xhotel = document.getElementById("xhotel");

                axios.post('/Edit/Hotel', formDataHotel, {
                    headers: { 'Content-Type': 'multipart/form-data' }
                })
                    .then(response => {
                        showAlert(response.data.message || 'Успешно отправлено!', 'success');

                        let option = document.createElement("option");
                        let lastItem = response.data[response.data.length - 1];
                        option.value = lastItem.name;
                        option.textContent = lastItem.name;
                        hotel.appendChild(option);
                        xhotel.appendChild(option);
                        location.reload();
                    })
                    .catch(error => {
                        const errorMsg = error.response?.data?.message || error.message || 'Произошла ошибка';
                        showAlert(errorMsg, 'danger');
                    });
            } else {
                formhotel.classList.add('was-validated');
            }
        });
    }

    const formbett = document.getElementById('BettForm');

    if (formbett) {
        formbett.addEventListener('submit', function (e) {
            e.preventDefault();
            const formDataBett = new FormData(formbett);
            const isValidBett = formbett.checkValidity();

            if (isValidBett) {
                axios.post('/Edit/Bett', formDataBett, {
                    headers: { 'Content-Type': 'multipart/form-data' }
                    
                }).then(response => {
                    location.reload();  
                }) 

            } else {
                formbett.classList.add('was-validated');
            }
        });
    }

    
    const formfirma = document.getElementById('FirmaForm');

    if (formfirma) {

        let firma = document.getElementById("RechnungsempfSelect");
        formfirma.addEventListener('submit', function (e) {
            e.preventDefault();
            const formDataBett = new FormData(formfirma);
            const isValidBett = formfirma.checkValidity();

            if (isValidBett) {
                axios.post('/Edit/Firma', formDataBett, {
                    headers: { 'Content-Type': 'multipart/form-data' }
                }).then(response => {

                   
                    let option = document.createElement("option");
                    let lastItem = response.data[response.data.length - 1];
                    option.value = lastItem.firmeName;
                    option.textContent = lastItem.firmeName;
                    firma.appendChild(option);
 
                })

            } else {
                formfirma.classList.add('was-validated');
            }
        });
    }

    const formkunde = document.getElementById('KundeForm');

    if (formkunde) {
        formkunde.addEventListener('submit', function (e) {
            e.preventDefault();
            const formDataBett = new FormData(formkunde);
            const isValidBett = formkunde.checkValidity();

            const gast = document.getElementById("NameGastSelect");

            if (isValidBett) {
                axios.post('/Edit/Gast', formDataBett, {
                    headers: { 'Content-Type': 'multipart/form-data' }
                }).then(response => {


                    let option = document.createElement("option");
                    let lastItem = response.data[response.data.length - 1];
                    option.value = lastItem.name;
                    option.textContent = lastItem.name;
                    gast.appendChild(option);

                })

            } else {
                formkunde.classList.add('was-validated');
            }
        });
    }
});
let coutBed = "";
function ZimmerSelect(value)
{
    let bed = document.getElementById('bettInput');

    axios.post('/Edit/SelectBad', { value: value })
        .then(response => {
            let allbed = "";
            response.data.forEach(x => {
                allbed += x.bettName +",";
            });
            bed.value = allbed;
            
            coutBed = bed.value;

            Price();
        })
        .catch(error => {
            console.error("Ошибка при отправке:", error);
        });
}
function Firma(value)
{
    let firma = document.getElementById('Kontakt');

    axios.post('/Edit/FirmaInfo', { FirmeName: value })
        .then(response => {

            console.log(response.data);
            firma.value = response.data.telefon;
           
          
        })
        .catch(error => {
            console.error("Ошибка при отправке:", error);
        });
}
function Gast(value) {
    let gast = document.getElementById('GastKontakt');

    axios.post('/Edit/GastInfo', { Name: value })
        .then(response => {

            console.log(response.data);
            gast.value = response.data.telefon;
        })
        .catch(error => {
            console.error("Ошибка при отправке:", error);
        });
}

let priceBet = 0;
let priceBetWochenende = 0;
let Zimmer = 0;


function filterOptionsByZimmer(zimmerList) {
    const zimmerSelect = document.getElementById("zimmerSelect");

    const allowedValues = zimmerList.map(z => z.toString());

    Array.from(zimmerSelect.options).forEach(option => {
        if (allowedValues.includes(option.value)) {
            option.hidden = false;
        } else {
            option.hidden = true;
        }
    });
}

function HotelSelect(value)
{

    axios.post('/Edit/HotelSelect', { Name: value })
        .then(response => {
            console.log(response.data);
            const zimmerValues = response.data.map(z => z.zimmer); 
            filterOptionsByZimmer(zimmerValues);
        })
        .catch(error => {
            console.error("Ошибка при отправке:", error);
        });

    Tarif(value);
}
function Tarif(value)
{
    axios.post('/Edit/Tarif', { Name: value })
        .then(response => {

            response.data.forEach(x => {

                if (x.name == "Bett")
                {
                    priceBet = x.price;
                    console.log(priceBet);
                }
                if (x.name == "Zimmer")
                {
                    Zimmer = x.price;
                    console.log(Zimmer);
                }
                if (x.name == "Wochenende")
                {
                    priceBetWochenende = x.price;
                    console.log(priceBetWochenende);
                   
                }

            });
            console.log(response.data);
           
            
        })
        .catch(error => {
            console.error("Ошибка при отправке:", error);
        });
}
function getWeekendsBetween(startDate, endDate) {
    let current = new Date(startDate);
    const end = new Date(endDate);
    const weekends = [];

    while (current <= end) {
        const day = current.getDay(); // 0 = Sunday, 6 = Saturday
        if (day === 0 || day === 6) {
            weekends.push(new Date(current)); // Копия даты
        }
        current.setDate(current.getDate() + 1);
    }

    return weekends;
}

function getDay(startDate, endDate) {

    let current = new Date(startDate);
    const end = new Date(endDate);
    const days = [];

    while (current <= end) {
        const dayOfWeek = current.getDay(); // 0 = Sunday, 6 = Saturday
        if (dayOfWeek !== 0 && dayOfWeek !== 6) {
            days.push(new Date(current)); // копия даты
        }
        current.setDate(current.getDate() + 1);
    }

    return days;

}
function getDayZimmer(startDate, endDate) {

    let current = new Date(startDate);
    const end = new Date(endDate);
    const days = [];

    while (current <= end) {

        days.push(new Date(current)); // копия даты
        
        current.setDate(current.getDate() + 1);
    }

    return days;

}
function Price()
{

    let pricefor_reserviert = 0;

    let zimmerSelect = document.getElementById("zimmerSelect");
    let musZahlen = document.getElementById("musZahlen");

    let zyhlen = document.getElementById("zyhlen");

    let reserviertvod = document.getElementById('reserviertvod');
    let reserviertbis = document.getElementById('reserviertbis');

    let bed = document.getElementById('bettInput');

    let elements = bed.value.split(',').filter(x => x.trim() !== "");
    const count = elements.length;

    const day = getDay(reserviertvod.value, reserviertbis.value);
    const weekendDates = getWeekendsBetween(reserviertvod.value, reserviertbis.value);

    const dayZimmer = getDayZimmer(reserviertvod.value, reserviertbis.value);


    console.log(count);

    if (bed.value != coutBed)
    {
        weekendDates.forEach(date => {
            pricefor_reserviert = pricefor_reserviert + (priceBetWochenende * count);
            console.log("wochenende");
        });

        day.forEach(date => {
            pricefor_reserviert = pricefor_reserviert + (priceBet * count);
            console.log("TagBet");
        });
    }
    else
    {
        dayZimmer.forEach(date => {
            pricefor_reserviert = pricefor_reserviert + Zimmer;
            console.log("TagZimmer");
        });
       
    }
    if (count==0) {
        dayZimmer.forEach(date => {
            pricefor_reserviert = pricefor_reserviert + Zimmer;
            console.log("TagZimmer");
        });
    }
  

    musZahlen.value = pricefor_reserviert;
}
let bezahl = 0;
function ZahlenSystem()
{
    let musZahlen = document.getElementById("musZahlen");

    bezahl = musZahlen.value;

    let zyhlen = document.getElementById("zyhlen");

    let zahlen = zyhlen.value;

    musZahlen.value = bezahl - zahlen;
}