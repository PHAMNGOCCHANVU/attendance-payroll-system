pipeline {
    agent any

    environment {
        // Cấu hình môi trường build
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        CONFIGURATION = 'Release'
        PROJECT_MAIN = 'cuoiky/cuoiky.csproj'
        PROJECT_TEST = 'cuoiky.Tests/cuoiky.Tests.csproj'
        TEST_RESULTS_DIR = 'TestResults'
    }

    options {
        // Giữ tối đa 10 bản build gần nhất để tiết kiệm dung lượng
        buildDiscarder(logRotator(numToKeepStr: '10'))
        // Hiển thị timestamp trong console log
        timestamps()
        // Timeout 30 phút phòng trường hợp treo build
        timeout(time: 30, unit: 'MINUTES')
    }

    stages {
        stage('Checkout SCM') {
            steps {
                echo '=== Bắt đầu Checkout mã nguồn ==='
                checkout scm
            }
        }

        stage('Restore Dependencies') {
            steps {
                echo '=== Đang khôi phục các NuGet Packages ==='
                bat "dotnet restore ${PROJECT_MAIN}"
                bat "dotnet restore ${PROJECT_TEST}"
            }
        }

        stage('Build Project') {
            steps {
                echo "=== Đang biên dịch ứng dụng với cấu hình ${CONFIGURATION} ==="
                bat "dotnet build ${PROJECT_MAIN} --configuration ${CONFIGURATION} --no-restore"
                bat "dotnet build ${PROJECT_TEST} --configuration ${CONFIGURATION} --no-restore"
            }
        }

        stage('Run Unit Tests') {
            steps {
                echo '=== Đang chạy kiểm thử tự động (Unit Tests) ==='
                bat "dotnet test ${PROJECT_TEST} --configuration ${CONFIGURATION} --no-restore --logger \"trx;LogFileName=mstest_results.trx\" --results-directory \"${TEST_RESULTS_DIR}\""
            }
        }

        stage('Archive Artifacts') {
            steps {
                echo '=== Lưu trữ file thực thi (Executable Artifacts) ==='
                archiveArtifacts artifacts: "cuoiky/bin/${CONFIGURATION}/**/*", 
                                 fingerprint: true, 
                                 allowEmptyArchive: true
            }
        }
    }

    post {
        always {
            // Lưu trữ file kết quả kiểm thử (.trx) vào artifacts của Jenkins (không phụ thuộc plugin ngoài)
            archiveArtifacts artifacts: "${TEST_RESULTS_DIR}/**/*.trx", 
                             fingerprint: true, 
                             allowEmptyArchive: true
        }
        success {
            echo " Pipeline hoàn thành thành công cho bản build #${BUILD_NUMBER}!"
        }
        failure {
            echo " Pipeline thất bại ở bản build #${BUILD_NUMBER}! Vui lòng kiểm tra console log."
        }
    }
}
