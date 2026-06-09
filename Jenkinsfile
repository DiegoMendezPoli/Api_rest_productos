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

        stage('Restore, Build & Publish') {
            steps {
                echo '== Restaurando, compilando y publicando dentro del build de Docker =='
                // El Dockerfile (multi-stage) ya hace dotnet restore + publish.
                // Compilamos la etapa "build" para validar que todo compile,
                // sin montar volumenes (que falla por Docker-in-Docker en Jenkins).
                sh 'docker build --target build -t $IMAGE_NAME:build-$IMAGE_TAG .'
            }
        }

        stage('Docker Build') {
            steps {
                echo '== Construyendo la imagen final de la API =='
                sh 'docker build -t $IMAGE_NAME:$IMAGE_TAG -t $IMAGE_NAME:latest .'
            }
        }
    }

    post {
        success { echo "BUILD OK - Imagen creada: ${IMAGE_NAME}:${IMAGE_TAG}" }
        failure { echo "BUILD FALLO - Revisa los logs de arriba" }
    }
}