/// <summary>
/// Represents the position of a Cell in a Table
/// </summary>
using System.Runtime.InteropServices;
using System.ComponentModel;
using System;

namespace ConnectableControls.SchedulerControl
{
    /// <MetaDataID>{fbc8ec60-2f16-42ab-a538-ff842538a834}</MetaDataID>
    [Serializable(), StructLayout(LayoutKind.Sequential)]
    public struct CellPos
    {
        #region Class Data

        /// <summary>
        /// Repsesents a null CellPos
        /// </summary>
        /// <MetaDataID>{e555b1a0-e936-4977-b627-c6a80dffd8f5}</MetaDataID>
        public static readonly CellPos Empty = new CellPos(-1, -1);

        /// <summary>
        /// The Row index of this CellPos
        /// </summary>
        /// <MetaDataID>{7b1c34ab-4a68-4366-b6dc-fcb8c0b5005c}</MetaDataID>
        private int row;

        /// <summary>
        /// The Column index of this CellPos
        /// </summary>
        /// <MetaDataID>{0bac48e4-ab31-4d2b-ae67-f0fe627deaa2}</MetaDataID>
        private int column;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CellPos class with the specified 
        /// row index and column index
        /// </summary>
        /// <param name="row">The Row index of the CellPos</param>
        /// <param name="column">The Column index of the CellPos</param>
        /// <MetaDataID>{27965dac-09c2-47be-abea-34878c6adfab}</MetaDataID>
        public CellPos(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Translates this CellPos by the specified amount
        /// </summary>
        /// <param name="TotalRows">The amount to offset the row index</param>
        /// <param name="columns">The amount to offset the column index</param>
        /// <MetaDataID>{78eda5f1-71d0-41c3-a3be-e9c3c8e145f9}</MetaDataID>
        public void Offset(int TotalRows, int columns)
        {
            this.row += TotalRows;
            this.column += columns;
        }


        /// <summary>
        /// Tests whether obj is a CellPos structure with the same values as 
        /// this CellPos structure
        /// </summary>
        /// <param name="obj">The Object to test</param>
        /// <returns>This method returns true if obj is a CellPos structure 
        /// and its Row and Column properties are equal to the corresponding 
        /// properties of this CellPos structure; otherwise, false</returns>
        /// <MetaDataID>{760c5ca0-454d-4fee-8e28-245924e00ab5}</MetaDataID>
        public override bool Equals(object obj)
        {
            if (!(obj is CellPos))
            {
                return false;
            }

            CellPos cellPos = (CellPos)obj;

            if (cellPos.Row == this.Row)
            {
                return (cellPos.Column == this.Column);
            }

            return false;
        }


        /// <summary>
        /// Returns the hash code for this CellPos structure
        /// </summary>
        /// <returns>An integer that represents the hashcode for this 
        /// CellPos</returns>
        /// <MetaDataID>{690e14df-ea5d-450f-8417-3a30d28bfd65}</MetaDataID>
        public override int GetHashCode()
        {
            return (this.Row ^ ((this.Column << 13) | (this.Column >> 0x13)));
        }


        /// <summary>
        /// Converts the attributes of this CellPos to a human-readable string
        /// </summary>
        /// <returns>A string that contains the row and column indexes of this 
        /// CellPos structure </returns>
        /// <MetaDataID>{f3f1a1bb-da10-4c72-85e0-92983c86dddf}</MetaDataID>
        public override string ToString()
        {
            return "CellPos: (" + this.Row + "," + this.Column + ")";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Row index of this CellPos
        /// </summary>
        /// <MetaDataID>{38775ab5-85df-4c41-a978-591e8637c6f5}</MetaDataID>
        public int Row
        {
            get
            {
                return this.row;
            }

            set
            {
                this.row = value;
            }
        }


        /// <summary>
        /// Gets or sets the Column index of this CellPos
        /// </summary>
        /// <MetaDataID>{710170e8-808c-4eee-818b-5dbd37a4ed19}</MetaDataID>
        public int Column
        {
            get
            {
                return this.column;
            }

            set
            {
                this.column = value;
            }
        }


        /// <summary>
        /// Tests whether any numeric properties of this CellPos have 
        /// values of -1
        /// </summary>
        /// <MetaDataID>{7d3ee5f4-beae-44ec-b653-6ab05dcd6838}</MetaDataID>
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return (this.Row == -1 || this.Column == -1);
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Tests whether two CellPos structures have equal Row and Column 
        /// properties
        /// </summary>
        /// <param name="left">The CellPos structure that is to the left 
        /// of the equality operator</param>
        /// <param name="right">The CellPos structure that is to the right 
        /// of the equality operator</param>
        /// <returns>This operator returns true if the two CellPos structures 
        /// have equal Row and Column properties</returns>
        /// <MetaDataID>{54529fc3-9d49-4d93-85c4-c59dacda550a}</MetaDataID>
        public static bool operator ==(CellPos left, CellPos right)
        {
            if (left.Row == right.Row)
            {
                return (left.Column == right.Column);
            }

            return false;
        }


        /// <summary>
        /// Tests whether two CellPos structures differ in their Row and 
        /// Column properties
        /// </summary>
        /// <param name="left">The CellPos structure that is to the left 
        /// of the equality operator</param>
        /// <param name="right">The CellPos structure that is to the right 
        /// of the equality operator</param>
        /// <returns>This operator returns true if any of the Row and Column 
        /// properties of the two CellPos structures are unequal; otherwise 
        /// false</returns>
        /// <MetaDataID>{633ca332-8694-4a5a-a052-3f86d9ed8a21}</MetaDataID>
        public static bool operator !=(CellPos left, CellPos right)
        {
            return !(left == right);
        }

        #endregion
    }
}