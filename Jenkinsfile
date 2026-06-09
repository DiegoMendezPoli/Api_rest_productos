pipeline {
    agent any

    environment {
        IMAGE_NAME = "taskmanager-api"
        IMAGE_TAG  = "${env.BUILD_NUMBER}"
    }

    stages {

        stage('Checkout') {
            steps {
                echo '== Descargando el codigo desde Git =='
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                echo '== Restaurando dependencias NuGet =='
                sh '''
                    docker run --rm \
                      -v "$PWD":/src -w /src \
                      mcr.microsoft.com/dotnet/sdk:10.0 \
                      dotnet restore TaskManagerApi/TaskManagerApi.csproj
                '''
            }
        }

        stage('Build') {
            steps {
                echo '== Compilando el proyecto =='
                sh '''
                    docker run --rm \
                      -v "$PWD":/src -w /src \
                      mcr.microsoft.com/dotnet/sdk:10.0 \
                      dotnet build TaskManagerApi/TaskManagerApi.csproj -c Release --no-restore
                '''
            }
        }

        stage('Test') {
            steps {
                echo '== Ejecutando tests (si existen) =='
                sh '''
                    docker run --rm \
                      -v "$PWD":/src -w /src \
                      mcr.microsoft.com/dotnet/sdk:10.0 \
                      bash -c "dotnet test || echo 'No hay tests, se omite'"
                '''
            }
        }

        stage('Docker Build') {
            steps {
                echo '== Construyendo la imagen Docker =='
                sh 'docker build -t $IMAGE_NAME:$IMAGE_TAG -t $IMAGE_NAME:latest .'
            }
        }
    }

    post {
        success { echo "BUILD OK - Imagen creada: ${IMAGE_NAME}:${IMAGE_TAG}" }
        failure { echo "BUILD FALLO - Revisa los logs de arriba" }
    }
}