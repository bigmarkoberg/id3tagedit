Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using open As New OpenFileDialog
                Select Case open.ShowDialog
                    Case DialogResult.Cancel
                        Return
                End Select

                Dim t = TagLib.File.Create(open.FileName)

                Debug.WriteLine(t)

            End Using
        Catch ex As Exception

        End Try
    End Sub
End Class
