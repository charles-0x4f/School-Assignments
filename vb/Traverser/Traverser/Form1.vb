'    Copyright 2013 Charles O.
'    Email: charles.0x4f@gmail.com
'    Github: https://github.com/charles-0x4f

'    This file is part of Traverser.

'    Foobar is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.

'    Foobar is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.

'    You should have received a copy of the GNU General Public License
'    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

Imports System.Math
Imports MySql
Imports MySql.Data.MySqlClient

Public Class Form1
    Dim nodeList As New List(Of Node)
    Dim connectionList As New Connections
    Private mode, priorNodeIndex As Integer
    Private nodePriori As Boolean

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set it so that we don't have a previous node we're connecting from
        nodePriori = False

        If ToolStripComboBox1.Items.Count > 0 Then
            ToolStripComboBox1.SelectedIndex = 0
            mode = 0
        End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        Dim circleNode As New Drawing.Pen(Color.BlueViolet, 5)
        Dim startNode As New Drawing.Pen(Color.Green, 5)
        Dim endNode As New Drawing.Pen(Color.Red, 5)
        Dim nodeMark As New Drawing.Pen(Color.Blue, 2)
        Dim untraversedConnection As New Drawing.Pen(Color.Black, 2)
        Dim traversedConnection As New Drawing.Pen(Color.Red, 2)

        Dim fontObj As Font = New System.Drawing.Font("Times", 12)

        ' Draw nodes
        For Each obj As Node In nodeList
            ' Draw nodes
            If obj.isStartNode = True Then
                e.Graphics.DrawEllipse(startNode, obj.getX, obj.getY, 20, 20)
            ElseIf obj.isEndNode = True Then
                e.Graphics.DrawEllipse(endNode, obj.getX, obj.getY, 20, 20)
            Else
                e.Graphics.DrawEllipse(circleNode, obj.getX, obj.getY, 20, 20)
            End If

            ' See if we're trying to connect nodes
            If nodePriori = True Then
                ' If we're going to connect this node to another, mark this one selected
                If obj.getID = nodeList.Item(priorNodeIndex).getID Then
                    e.Graphics.DrawRectangle(nodeMark, (obj.getX - 10), (obj.getY - 10), 5, 5)
                End If
            End If

            ' Draw NodeID with the nodes
            'e.Graphics.DrawString(obj.getID.ToString, fontObj, Brushes.Black, New System.Drawing.PointF(obj.getX - 15, obj.getY - 15))
        Next

        Dim tempList As New Dictionary(Of Traverser.Connection, Boolean)
        connectionList.getConnectionList(tempList)

        ' Draw connecting lines
        If tempList.Count > 0 Then
            For counter = 0 To tempList.Count
                Dim temp As New Connection
                Dim tempNode1, tempNode2 As Node

                temp = tempList.Keys(counter)
                tempNode1 = getNodeFromID(temp._first)
                tempNode2 = getNodeFromID(temp._second)

                ' draw connection, finally
                If connectionList.isTraveresed(tempNode1.getID, tempNode2.getID) = True Then
                    e.Graphics.DrawLine(traversedConnection, (tempNode1.getX + 10), (tempNode1.getY + 10), _
                                        (tempNode2.getX + 10), (tempNode2.getY + 10))
                Else
                    e.Graphics.DrawLine(untraversedConnection, (tempNode1.getX + 10), (tempNode1.getY + 10), _
                                        (tempNode2.getX + 10), (tempNode2.getY + 10))
                End If
            Next
        End If
    End Sub

    Private Sub click_Handler(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.Click
        Select Case mode

            Case 0
                ' Add node
                Dim temp As New Node(e.X, e.Y)
                nodeList.Add(temp)

            Case 1
                ' Add connection

                ' Get node under mouse when clicked
                Dim nodeIndex As Integer = checkNodeCollision(e.X, e.Y)

                ' If there's a node there
                If nodeIndex <> -1 Then
                    ' If this is the first node in a connection
                    If nodePriori = False Then
                        nodePriori = True
                        priorNodeIndex = nodeIndex
                        ' If this is the second node in a connection
                    Else
                        'connect nodes
                        Dim prior As Node = nodeList.Item(priorNodeIndex)
                        Dim current As Node = nodeList.Item(nodeIndex)

                        Dim distance As Double = getDistance(prior.getX, prior.getY, current.getX, current.getX)

                        prior.connectNode(current.getID, distance, connectionList)
                        current.connectNode(prior.getID, distance, connectionList)

                        nodePriori = False
                    End If
                End If

            Case 2
                ' Remove node

                Dim nodeIndex As Integer = checkNodeCollision(e.X, e.Y)

                If nodeIndex <> -1 Then
                    nodeList.Item(nodeIndex).removeNodeConnections(connectionList)
                    nodeList.RemoveAt(nodeIndex)
                End If

            Case 3
                ' Remove all connections from node

                Dim nodeIndex As Integer = checkNodeCollision(e.X, e.Y)

                If nodeIndex <> -1 Then
                    nodeList.Item(nodeIndex).removeNodeConnections(connectionList)
                End If

            Case 4
                ' Set start node

                Dim nodeIndex As Integer = checkNodeCollision(e.X, e.Y)

                If nodeIndex <> -1 Then

                    ' Remove start node status from all nodes
                    For Each obj In nodeList
                        obj.isStartNode = False
                    Next

                    ' Set start flag in selected node
                    nodeList.Item(nodeIndex).isStartNode = True
                    nodeList.Item(nodeIndex).isEndNode = False
                End If

            Case 5
                ' Set end node

                Dim nodeIndex As Integer = checkNodeCollision(e.X, e.Y)

                If nodeIndex <> -1 Then

                    ' Remove end node status from all nodes
                    For Each obj In nodeList
                        obj.isEndNode = False
                    Next

                    ' Set end flag in selected node
                    nodeList.Item(nodeIndex).isEndNode = True
                    nodeList.Item(nodeIndex).isStartNode = False
                End If
        End Select

        Panel1.Refresh()

    End Sub

    ' Everytime the drop down menu is changed, change mode
    Private Sub ToolStripComboBox1_Changed(sender As Object, e As EventArgs) Handles ToolStripComboBox1.TextChanged
        mode = ToolStripComboBox1.SelectedIndex
    End Sub

    ' Find the node under the mouse pointer on click
    Private Function checkNodeCollision(ByVal x As Integer, ByVal y As Integer)
        For counter As Integer = 0 To (nodeList.Count - 1)
            Dim obj As Node = nodeList.Item(counter)

            If ((x >= obj.getX) And (x <= (obj.getX + 20))) Then
                If ((y >= obj.getY) And (y <= (obj.getY + 20))) Then
                    Return counter
                End If
            End If
        Next

        Return -1
    End Function

    ' Finds the distance between two nodes
    Public Function getDistance(ByVal x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer) As Double
        'Use distance formula to calculate distance between two nodes, to be used in pathfinding
        ' sqrt(((x2 - x1)^2) + ((y2 - y1)^2))

        Dim part1 As Double = ((x2 - x1) ^ 2)
        Dim part2 As Double = ((y2 - y1) ^ 2)

        Return Sqrt(part1 + part2)
    End Function

    Public Function getNodeFromID(ByVal id As Integer) As Node
        For Each obj In nodeList
            If obj.getID = id Then
                Return obj
            End If
        Next

        'MessageBox.Show("uh oh")
        Return New Node(0, 0)
    End Function

    Private Sub TraverseButton_Click(sender As Object, e As EventArgs) Handles TraverseButton.Click
        Dim startFound As Boolean = False, endFound As Boolean = False

        ' Traverse through node list and find start and end nodes
        For Each obj In nodeList
            If obj.isStartNode = True Then
                startFound = True
                Continue For
            ElseIf obj.isEndNode = True Then
                endFound = True
                Continue For
            End If
        Next

        ' If start or end nodes not set
        If startFound = False Or endFound = False Then
            MessageBox.Show("No start or end node found")
            Return
        Else
            ' Path find
            Dim Astar As New AStar(connectionList, nodeList)
            If Astar.PathFind() = False Then
                MessageBox.Show("Pathfinding failed")
            Else
                Panel1.Refresh()
            End If
        End If
    End Sub

    Private Sub QuitToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles QuitToolStripMenuItem1.Click
        Application.Exit()
    End Sub

    ' Handles the saving to database
    Private Sub QuitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitToolStripMenuItem.Click
        Dim databaseName As String
        'Dim saveDialog As New System.Windows.Forms.SaveFileDialog

        databaseName = InputBox("Enter 1 word database name (REMEMBER THIS)", "Enter DB name")

        If databaseName.Length = 0 Then
            Return
        End If

        ' Get the raw database name without extension
        Dim databaseNameShort As String = databaseName.Substring(databaseName.LastIndexOf("\") + 1)

        If databaseNameShort.Contains(".") Then
            databaseNameShort = databaseName.Remove(databaseNameShort.LastIndexOf("."), databaseNameShort.Length)
        End If

        ' Assume default MySQL installation; user: root password: pass
        'Database=" & databaseNameShort & ";
        Dim sqlData As String = "Data Source=localhost;Port=3306;User id=root;Password=pass"

        Dim sqlConnection As MySqlConnection
        Dim commandString, result As String
        Dim command As New MySqlCommand()

        Try
            sqlConnection = New MySqlConnection(sqlData)
            sqlConnection.Open()

            ' Delete any database by this name (permanent)
            commandString = "DROP DATABASE IF EXISTS " & databaseNameShort & ";"
            command = New MySqlCommand(commandString, sqlConnection)
            result = Convert.ToString(command.ExecuteScalar())

            ' Create new database by same name
            commandString = "CREATE DATABASE IF NOT EXISTS " & databaseNameShort & ";"
            command = New MySqlCommand(commandString, sqlConnection)
            result = Convert.ToString(command.ExecuteScalar())

            ' Select our clean database
            commandString = "USE " & databaseNameShort & ";"
            command = New MySqlCommand(commandString, sqlConnection)
            result = Convert.ToString(command.ExecuteScalar())

            ' Create node table
            commandString = "CREATE TABLE node (id INT(32), x INT(32), y INT(32), connectedToCount INT(32), " _
                    & "isStartNode INT(32), isEndNode INT(32), primary KEY (id));"
            command = New MySqlCommand(commandString, sqlConnection)
            result = Convert.ToString(command.ExecuteScalar())

            ' Create connection table (each node has a list of this table)
            commandString = "CREATE TABLE connectsTable (id INT(32), nodeID INT(32), distance DOUBLE);"
            command = New MySqlCommand(commandString, sqlConnection)
            result = Convert.ToString(command.ExecuteScalar())

            ' Create global unique connections table (the Connections object)
            commandString = "CREATE TABLE connections (first INT(32), second INT(32), RID int(11) NOT NULL auto_increment," _
                    & "primary KEY (RID));"
            command = New MySqlCommand(commandString, sqlConnection)
            result = Convert.ToString(command.ExecuteScalar())

            ' Iterate through node list
            For Each obj In nodeList
                Dim connectsTable As Dictionary(Of Integer, Double) = obj.getConnectsTable()
                Dim id As Integer = obj.getID
                Dim isStart As Integer = 0, isEnd As Integer = 0

                If obj.isStartNode = True Then
                    isStart = 1
                ElseIf obj.isEndNode = True Then
                    isEnd = 1
                End If

                ' Fill in the connects table
                For counter As Integer = 0 To (connectsTable.Count - 1)
                    commandString = "INSERT INTO connectsTable (id, nodeID, distance) VALUES" _
                        & "('" & id & "', '" & connectsTable.Keys(counter) & "', '"
                    commandString = commandString & connectsTable.Values(counter) & "');"
                    command = New MySqlCommand(commandString, sqlConnection)
                    result = Convert.ToString(command.ExecuteScalar())
                Next

                ' Add each node object to the node table
                commandString = "INSERT INTO node (id, x, y, connectedToCount, isStartNode, isEndNode) " _
                        & "VALUES('" & id & "', '" & obj.getX & "', '" & obj.getY & "', '" & obj.getConnectionCount & "', '" _
                        & isStart & "', '" & isEnd & "');"
                command = New MySqlCommand(commandString, sqlConnection)
                result = Convert.ToString(command.ExecuteScalar())
            Next

            ' Iterate through connections list
            Dim connectionsListTable As New Dictionary(Of Connection, Boolean)
            connectionList.getConnectionList(connectionsListTable)

            For counter As Integer = 0 To (connectionsListTable.Count - 1)
                Dim connection As New Connection
                connection = connectionsListTable.Keys(counter)

                commandString = "INSERT INTO connections (first, second) VALUES('" & connection._first & "', '" & connection._second & "');"
                command = New MySqlCommand(commandString, sqlConnection)
                result = Convert.ToString(command.ExecuteScalar())
            Next

        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
            MessageBox.Show("Save failed")
        Finally
            sqlConnection.Close()
        End Try
    End Sub

    ' Handles loading from database
    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Dim databaseName As String = ""
        'Dim loadDialog As New System.Windows.Forms.SaveFileDialog

        databaseName = InputBox("Enter 1 word database name", "Enter DB name")

        'If loadDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
        'databaseName = loadDialog.FileName()
        'End If

        ' Get database name
        databaseName = databaseName.Substring(databaseName.LastIndexOf("\") + 1)

        ' Assume default MySQL localhost installation; user: root password: pass
        Dim sqlData As String = "Data Source=localhost;Port=3306;User id=root;Password=pass"

        Dim sqlConnection As MySqlConnection
        Dim commandString, result As String
        Dim command As New MySqlCommand()
        Dim reader As MySqlDataReader

        Try
            sqlConnection = New MySqlConnection(sqlData)
            sqlConnection.Open()

            commandString = "USE " & databaseName & ";"
            command = New MySqlCommand(commandString, sqlConnection)
            result = Convert.ToString(command.ExecuteScalar())

            commandString = "SELECT * FROM node"
            command = New MySqlCommand(commandString, sqlConnection)
            reader = command.ExecuteReader()

            ' Read in all nodes from database and add them to list
            While reader.Read()
                Dim id, x, y, isStart, isEnd As Integer
                Dim tempNode As Node

                id = reader.GetInt32(0)
                x = reader.GetInt32(1)
                y = reader.GetInt32(2)
                isStart = reader.GetInt32(4)
                isEnd = reader.GetInt32(5)

                tempNode = New Node(x, y)
                tempNode.setID(id)

                If isStart = 1 Then
                    tempNode.isStartNode = True
                ElseIf isEnd = 1 Then
                    tempNode.isEndNode = True
                End If

                nodeList.Add(tempNode)
            End While

            reader.Close()

            commandString = "SELECT * FROM connectsTable"
            command = New MySqlCommand(commandString, sqlConnection)
            reader = command.ExecuteReader()

            ' Read in all inter node connections (per node, not the global connection list) from database
            While reader.Read()
                Dim originalNodeID, connectedNodeID As Integer
                Dim distance As Double

                originalNodeID = reader.GetInt32(0)
                connectedNodeID = reader.GetInt32(1)
                distance = reader.GetDouble(2)

                getNodeFromID(originalNodeID).connectNode(connectedNodeID, distance, connectionList)
            End While

            reader.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            sqlConnection.Close()
            Panel1.Refresh()
        End Try
    End Sub
End Class
