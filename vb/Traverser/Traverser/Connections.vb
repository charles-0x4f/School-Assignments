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

Public Structure Connection
    Public _first As Integer
    Public _second As Integer

    ' Used for determining if a node is in a particular set of connections
    Public Sub swap()
        Dim temp As Integer = _first
        _first = _second
        _second = temp
    End Sub

    ' See if this connection struct object contains the ID we want
    Public Function contains(ByVal value As Integer) As Boolean
        If (_first = value) Or (_second = value) Then
            Return True
        End If

        Return False
    End Function

    ' Get the node ID of the node connected to the node we're looking for
    Public Function getOtherNode(ByVal ID As Integer) As Integer
        If _first = ID Then
            Return _second
        ElseIf _second = ID Then
            Return _first
        Else
            Return -1
        End If
    End Function
End Structure

Public Class Connections
    Dim connectionList As New Dictionary(Of Connection, Boolean)

    Public Sub add(ByVal firstNode As Integer, ByVal secondNode As Integer, ByVal traversed As Boolean)
        Dim tempConnection As New Connection
        tempConnection._first = firstNode
        tempConnection._second = secondNode

        ' Check if connection has already been made
        If connectionList.ContainsKey(tempConnection) Then
            Return
        Else
            ' If not, swap the node list and see if the reverse connection has been made
            tempConnection.swap()

            If connectionList.ContainsKey(tempConnection) Then
                Return
            Else
                ' Swap Connection to default values and add to the connection list
                tempConnection.swap()
                connectionList.Add(tempConnection, traversed)
            End If
        End If
    End Sub

    ' Return an object of this class
    Public Sub getConnectionList(ByRef connect As Dictionary(Of Connection, Boolean))
        connect = connectionList
    End Sub

    ' Removes all the connections this node has
    Public Function removeAllConnections(ByVal nodeID As Integer) As Boolean
        Dim tempConnection As Connection
        Dim returnValue As Boolean
        Dim removeList As New List(Of Integer)

        ' VB doesn't seem to like iterating over a changing collection so we're
        ' just going to mark the node connections that need to be removed
        For counter As Integer = 0 To connectionList.Count
            tempConnection = connectionList.Keys(counter)

            If tempConnection.contains(nodeID) = True Then
                removeList.Add(counter)
                returnValue = True
            End If
        Next

        ' Remove
        For counter As Integer = (removeList.Count - 1) To 0 Step -1
            tempConnection = connectionList.Keys(removeList.Item(counter))
            connectionList.Remove(tempConnection)
        Next

        If returnValue = True Then
            Return True
        Else
            Return False
        End If
    End Function

    ' Return number of nodes connected to this node
    Public Function getNodeConnectionCount(ByVal nodeID) As Integer
        Dim count As Integer = 0

        For counter As Integer = 0 To connectionList.Count
            Dim tempConnection As Connection = connectionList.Keys(count)

            If tempConnection.contains(nodeID) = True Then
                count += 1
            End If
        Next

        Return count
    End Function

    ' Return list of nodes connected to this node
    Public Function getListOfConnectedNodes(ByVal nodeID As Integer) As List(Of Integer)
        Dim nodeList As New List(Of Integer)

        For counter As Integer = 0 To connectionList.Count
            Dim tempConnection As Connection = connectionList.Keys(counter)

            If tempConnection.contains(nodeID) = True Then
                nodeList.Add(tempConnection.getOtherNode(nodeID))
            End If
        Next

        Return nodeList
    End Function

    ' Set the connection path to traversed(red)
    Public Sub setTraversed(ByVal nodeID1 As Integer, ByVal nodeID2 As Integer)
        Dim tempConnection As New Connection

        tempConnection._first = nodeID1
        tempConnection._second = nodeID2

        If connectionList.ContainsKey(tempConnection) = True Then
            connectionList.Item(tempConnection) = True
        Else
            tempConnection.swap()
            If connectionList.ContainsKey(tempConnection) = True Then
                connectionList.Item(tempConnection) = True
            Else
                Return
            End If
        End If
    End Sub

    ' Returns true or false if this path has been traversed(red)
    Public Function isTraveresed(ByVal nodeID1 As Integer, ByVal nodeID2 As Integer) As Boolean
        Dim tempConnection As New Connection

        tempConnection._first = nodeID1
        tempConnection._second = nodeID2

        If connectionList.ContainsKey(tempConnection) = True Then
            Return connectionList.Item(tempConnection)
        Else
            tempConnection.swap()

            If connectionList.ContainsKey(tempConnection) = True Then
                Return connectionList.Item(tempConnection)
            End If
        End If

        Return False
    End Function
End Class
