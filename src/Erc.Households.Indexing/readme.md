Build docker image
cd ..
docker build -t erc/households/indexing . -f Erc.Households.Indexing/Dockerfile
docker tag erc/households/indexing 10.67.1.238:5000/erc/households/indexing

Restart microservices
sudo docker-compose pull
sudo systemctl start microservices.service