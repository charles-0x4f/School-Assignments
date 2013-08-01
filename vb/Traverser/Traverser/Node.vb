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

Public Class Node
    Private nodeID As Integer
    Private xLocation, yLocation As Integer
    Private connectedToCount As Integer
    Public isStartNode, isEndNode As Boolean
    Private connects As New Dictionary(Of Integer, Double)

    Public Sub New(ByVal x As Integer, ByVal y As Integer)
        xLocation = x
        yLocation = y
        connectedToCount = 0

        isStartNode = False
        isEndNode = False

        nodeID = System.Guid.NewGuid.GetHashCode

        'Dim connections As New Dictionary(Of Integer, Double)
    End Sub

    Public Function getX() As Integer
        Return xLocation
    End Function

    Public Function getY() As Integer
        Return yLocation
    End Function

    Public Function getID() As Integer
        Return nodeID
    End Function

    Public Sub setID(ByVal id As Integer)
        ' Used for loading from database
        nodeID = id
    End Sub

    Public Function getConnectionCount() As Integer
        Return connectedToCount
    End Function

    ' Set up the connection between nodes, both internally in node class and externally through the connections class
    Public Sub connectNode(ByVal ID As Integer, ByVal distance As Double, ByRef connectionList As Connections)
        If connects.ContainsKey(ID) = False Then
            connects.Add(ID, distance)
            connectedToCount += 1
        End If

        connectionList.add(nodeID, ID, False)
    End Sub

    ' Remove node connections in node class and connections class
    Public Function removeNodeConnections(ByRef connectionList As Connections) As Boolean
        If connectionList.removeAllConnections(nodeID) = True Then
            connectedToCount -= 1
            Return True
        End If

        Return False
    End Function

    ' Get the distance between this node and another
    Public Function getDistanceFromConnectedNode(ByVal ID) As Double
        If connects.ContainsKey(ID) = True Then
            Return connects.Item(ID)
        End If

        Return -1
    End Function

    ' Return the connections object
    Public Function getConnectsTable() As Dictionary(Of Integer, Double)
        Return connects
    End Function
End Class
