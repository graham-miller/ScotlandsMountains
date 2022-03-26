cd ../../scotlandsmountains.github.io

git rm -r *

cp -R ../ScotlandsMountains/UI/dist/ui/* .

git add .

git commit -m "Deployment"

git push
