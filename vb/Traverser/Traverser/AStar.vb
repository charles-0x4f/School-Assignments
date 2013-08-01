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

Public Structure NodeCost
    Public _G, _H, _F As Double
    Public _parent As Integer

    Public Function calculate() As Double
        _F = _G + _H
        Return _F
    End Function
End Structure

' Implementation of A* algorithm
Public Class AStar
    Private nodeConnections As Connections
    Private connectionList As Dictionary(Of Connection, Boolean)
    Private nodeList As New List(Of Node)
    Dim openList, closedList As New Dictionary(Of Integer, NodeCost)

    Private currentG As Double = 0
    Private StartNode, endNode As Integer

    Public Sub New(ByRef connectionCollection As Connections, ByRef nodes As List(Of Node))
        Dim tempCost As New NodeCost
        nodeConnections = connectionCollection
        connectionCollection.getConnectionList(connectionList)
        nodeList = nodes

        ' Get start and end nodes; add start node to open list with costs
        For Each obj In nodeList
            If obj.isEndNode = True Then
                endNode = obj.getID()
            End If

            If obj.isStartNode = True Then
                StartNode = obj.getID

                tempCost._G = 0
                tempCost._H = 0
                tempCost._parent = 0
            End If

            If endNode <> 0 Then
                If StartNode <> 0 Then
                    Dim tempNode As Node = Form1.getNodeFromID(StartNode)
                    Dim tempNode2 As Node = Form1.getNodeFromID(endNode)

                    tempCost._F = Form1.getDistance(tempNode.getX, tempNode.getY, tempNode2.getX, tempNode2.getY)
                End If
            End If
        Next

        openList.Add(StartNode, tempCost)
    End Sub

    Public Function PathFind() As Boolean
        Dim currentNode, previousNodeID As Integer
        Dim lowestScore As Double = -1

        Dim winningPath As New List(Of Integer)
        Dim winningPathIndex As Integer = 0

        While openList.Count > 0
            ' Find lowest F score in all of the open list
            For Each obj In openList

                Dim nodeID As Integer = obj.Key()
                Dim nodeStats As NodeCost = obj.Value()

                If (lowestScore = -1) Or (nodeStats._F < lowestScore) Then
                    lowestScore = nodeStats._F
                    previousNodeID = currentNode
                    currentNode = nodeID
                    currentG = nodeStats._G
                End If


            Next

            lowestScore = -1

            ' If the current node is the end node
            If currentNode = endNode Then
                Dim currentCost As NodeCost
                Dim currentID As Integer = currentNode, parentID As Integer

                'Move end node to closed list
                closedList.Add(currentNode, openList.Item(currentNode))
                openList.Remove(currentNode)

                Do
                    currentCost = closedList.Item(currentID)
                    parentID = currentCost._parent

                    nodeConnections.setTraversed(currentID, parentID)

                    currentID = parentID
                Loop While currentCost._parent <> 0

                Return True
            End If

            ' Add current to closed list
            closedList.Add(currentNode, openList.Item(currentNode))
            ' Remove current from open list
            openList.Remove(currentNode)

            Dim nodesConnected As List(Of Integer) = nodeConnections.getListOfConnectedNodes(currentNode)

            ' For each node connected to current node
            If nodesConnected.Count > 0 Then
                For Each obj In nodesConnected
                    ' If node not on open list
                    If openList.ContainsKey(obj) = True Then

                        ' If neighbor already on open list, see if it's shorter
                        If openList.Item(obj)._G > currentG Then
                            Dim tempNode As NodeCost = openList.Item(obj)
                            Dim currentNodeObj As Node = Form1.getNodeFromID(currentNode)

                            tempNode._parent = currentNode
                            tempNode._G = currentNodeObj.getDistanceFromConnectedNode(obj) + currentG
                            tempNode._F = tempNode.calculate()

                            openList.Item(obj) = tempNode
                        End If

                        Continue For
                        ' If not not on closed list
                    ElseIf closedList.ContainsKey(obj) = True Then
                        Continue For
                    Else
                        Dim tempCost As New NodeCost

                        Dim currentNodeObj As Node = Form1.getNodeFromID(currentNode)
                        Dim connectedNodeObj As Node = Form1.getNodeFromID(obj)
                        Dim endNodeObj As Node = Form1.getNodeFromID(endNode)

                        ' Calculate score of the connected node
                        ' G = current node's G + the distance from current node to the connected node
                        tempCost._G = currentNodeObj.getDistanceFromConnectedNode(obj) + currentG
                        ' H = estimated distance from connected node to the end node
                        tempCost._H = Form1.getDistance(connectedNodeObj.getX, connectedNodeObj.getY, endNodeObj.getX, endNodeObj.getY)
                        ' Calculate F (G+H)
                        tempCost._F = tempCost.calculate()
                        tempCost._parent = currentNode

                        ' Add node and score to open list
                        openList.Add(obj, tempCost)
                    End If
                Next
            End If

        End While

                ' Return false
                Return False
    End Function

End Class
