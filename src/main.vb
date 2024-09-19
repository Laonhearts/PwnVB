Imports System.Net.Sockets
Imports System.IO
Imports Renci.SshNet

Module Program

    Sub Main()

        Console.WriteLine("연결 방식 선택:")
        Console.WriteLine("1. Netcat (TCP)")
        Console.WriteLine("2. SSH")

        Dim choice As Integer = Integer.Parse(Console.ReadLine())

        Console.Write("서버 주소를 입력하세요: ")

        Dim serverAddress As String = Console.ReadLine()

        Console.Write("포트 번호를 입력하세요: ")

        Dim portNumber As Integer = Integer.Parse(Console.ReadLine())

        If choice = 1 Then

            ' Netcat 연결 시도 (TCP)
            ConnectToNetcat(serverAddress, portNumber)

        ElseIf choice = 2 Then

            ' SSH 연결 시도
            Console.Write("사용자 이름을 입력하세요: ")

            Dim username As String = Console.ReadLine()

            Console.Write("비밀번호를 입력하세요: ")

            Dim password As String = Console.ReadLine()

            ConnectToSSH(serverAddress, portNumber, username, password)

        Else

            Console.WriteLine("잘못된 선택입니다.")

        End If

        ' 프로그램 종료를 기다림
        Console.WriteLine("아무 키나 누르면 프로그램이 종료됩니다.")
        Console.ReadKey()

    End Sub

    ' Netcat(TCP) 서버에 연결하는 함수
    Private Sub ConnectToNetcat(serverAddress As String, portNumber As Integer)

        Try

            ' 서버에 연결 시도
            Using client As New TcpClient(serverAddress, portNumber)

                Console.WriteLine("Netcat 서버에 연결되었습니다.")

                ' 네트워크 스트림을 가져옴
                Dim networkStream As NetworkStream = client.GetStream()

                ' 서버로 메시지를 전송
                Dim writer As New StreamWriter(networkStream)

                writer.WriteLine("Hello, Netcat Server!")
                writer.Flush() ' 스트림에 데이터를 보내기 위해 Flush 호출

                ' 서버로부터 응답을 읽음
                Dim reader As New StreamReader(networkStream)
                Dim response As String = reader.ReadLine()

                Console.WriteLine("Netcat 서버로부터 받은 메시지: " & response)

                ' 연결 종료
                writer.Close()
                reader.Close()
                networkStream.Close()
            
            End Using
        
        Catch ex As SocketException
        
            Console.WriteLine("서버에 연결할 수 없습니다: " & ex.Message)
        
        Catch ex As Exception
        
            Console.WriteLine("오류 발생: " & ex.Message)
        
        End Try
    
    End Sub

    ' SSH 서버에 연결하는 함수
    Private Sub ConnectToSSH(serverAddress As String, portNumber As Integer, username As String, password As String)
    
        Try
    
            ' SSH 연결 시도
            Using client As New SshClient(serverAddress, portNumber, username, password)
    
                client.Connect()

                If client.IsConnected Then
    
                    Console.WriteLine("SSH 서버에 연결되었습니다.")

                    ' SSH 세션을 통해 명령어 실행 (예: 'hostname' 명령어 실행)
                    Dim cmd = client.CreateCommand("hostname")
                    Dim result = cmd.Execute()
    
                    Console.WriteLine("서버의 호스트 이름: " & result)
    
                End If

                client.Disconnect()
    
            End Using
    
        Catch ex As Exception
    
            Console.WriteLine("SSH 연결 실패: " & ex.Message)
    
        End Try
    
    End Sub

End Module
