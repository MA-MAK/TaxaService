	
DOCKER IMAGES KOMMANDOER TIL SERVICES:

AUTHSERVICE:

docker build -t authservice_image -f Dockerfile .

docker tag authservice_image mortenlundmikkelsen/authservice

docker push mortenlundmikkelsen/authservice


BOOKINGSERVICE:

docker build -t bookingservice_image -f Dockerfile .

docker tag bookingservice_image mortenlundmikkelsen/bookingservice

docker push mortenlundmikkelsen/bookingservice


MAINTENANCESERVICE:

docker build -t maintenanceservice_image -f Dockerfile .

docker tag maintenanceservice_image mortenlundmikkelsen/maintenanceservice

docker push mortenlundmikkelsen/maintenanceservice


PLANSERVICE:

docker build -t planservice_image -f Dockerfile .

docker tag planservice_image mortenlundmikkelsen/planservice

docker push mortenlundmikkelsen/planservice


VEHICLESERVICE:

docker build -t vehicleservice_image -f Dockerfile .

docker tag vehicleservice_image mortenlundmikkelsen/vehicleservice

docker push mortenlundmikkelsen/vehicleservice





******** OLD - NO LONGER IN USE ********

************************** How to Containerize services **************************

Taxa selskab

- Bygger container image 
docker build -t planservice_image  -f Dockerfile .
docker build -t bookingservice_image  -f Dockerfile .

- Opretter nyt tag (kopi) m. Kontonavn er version

docker tag planservice_image mortenlundmikkelsen/planservice
docker tag bookingservice_image mortenlundmikkelsen/bookingservice

- Logger ind

docker login

- Pusher image til repo’et
docker push mortenlundmikkelsen/planservice

docker push mortenlundmikkelsen/bookingservice


- Opretter lokal container af image

docker run -d -p 5001:80 --name planservice_container mortenlundmikkelsen/planservice


-  Opretter lokale container og forbinder til volume

docker run -d --name dev-rabbit --hostname rabbitmq-dev -p 15672:15672 -p 5672:5672 --network=TaxaNet rabbitmq:management 

docker run -d -p 5002:80 -v TaxaPlans:/srv/plans --env rabbitmqHost=”dev-rabbit” --env PlanPath=“//srv/plans” --name planservice_container mortenlundmikkelsen/planservice:latest --network=TaxaNet

docker run -d -p 5001:80 -v TaxaPlans:/srv/plans --env rabbitmqHost=” dev-rabbit” --env PlanPath=“//srv/plans” --name bookingservice_container mortenlundmikkelsen/bookingservice:latest --network=TaxaNet


						
							
					
							
	**************************	Modul 6 - Docker **************************

Opgave M6.01

Henter Alpine container-image - fra Dockers offentlige repo
- docker pull alpine

Kører image - vi navngiver ikke så der oprettes automatisk en container
-docker run -it alpine

Genstarter container
Vælger på liste
- docker ps -a

Starter containeren op igen
- docker start -I <din containers-navn>

Se resten i opgave beskrivelse



 Når vi skal have vores service op og kører på docker


Bygger container image **
- docker build -t service_a_image  -f Dockerfile .

Opretter nyt tag (kopi) m. Kontonavn er version
- docker tag service_a_image mortenlundmikkelsen/service_a

docker tag planservice_image mortenlundmikkelsen/planservice

docker tag authservice_image mortenlundmikkelsen/authservice


Logger ind
- docker login

Pusher image til repo’et
- docker push mortenlundmikkelsen/service_a

docker push mortenlundmikkelsen/planservice

Opretter lokal container af image
- docker run -d -p 5001:80 --name service_a_container mortenlundmikkelsen/service_a


docker run -d -p 5001:80 --name planservice_container mortenlundmikkelsen/planservice



Opgave M6.03

docker run -d -p 5001:80 -v images:/srv/images --env ImagePath="//srv/images" --name list_image_service mortenlundmikkelsen/list_images_service:latest


docker run -d -p 5001:80 -v TaxaPlans:/srv/plans --env PlanPath="//srv/plans” --name planservice_container mortenlundmikkelsen/planservice:latest

docker run -d -p 5001:80 -v TaxaPlans:/srv/plans --env PlanPath ="//srv/plans” --name bookingservice_container mortenlundmikkelsen/bookingservice:latest















