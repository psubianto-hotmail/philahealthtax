# philahealthtax
Phila Health Exercise

Clone the project from GitHub
- type command : git clone https://github.com/psubianto-hotmail/philahealthtax.git

To deploy to the Docker for Linux
- go to the solution folder
- type command : docker build -t philahealthtax .
- type command : docker run -it --rm -p 5000:80 --name philahealthtaxapi philahealthtax

To access the Web API, use a browser and navigate to :
- http://localhost:5000/api/PropertyTax/183189500
