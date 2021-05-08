#region Copyright (c) 2008 Microsoft Corporation.  All rights reserved.
// Copyright (c) 2008 Microsoft Corporation.  All rights reserved.
// 
// THIS SOFTWARE COMES "AS IS", WITH NO WARRANTIES.  THIS
// MEANS NO EXPRESS, IMPLIED OR STATUTORY WARRANTY, INCLUDING
// WITHOUT LIMITATION, WARRANTIES OF MERCHANTABILITY OR FITNESS
// FOR A PARTICULAR PURPOSE OR ANY WARRANTY OF TITLE OR
// NON-INFRINGEMENT.
//
// MICROSOFT WILL NOT BE LIABLE FOR ANY DAMAGES RELATED TO
// THE SOFTWARE, INCLUDING DIRECT, INDIRECT, SPECIAL,
// CONSEQUENTIAL OR INCIDENTAL DAMAGES, TO THE MAXIMUM EXTENT
// THE LAW PERMITS, NO MATTER WHAT LEGAL THEORY IT IS
// BASED ON.
#endregion

using System.Linq.Expressions;

namespace OOAdvantech.Linq
{
  internal class QueryInfo
  {
    public QueryInfo(ResultShape shape, string queryText)
    {
      ResultShape = shape;
      QueryText = queryText;
    }

    /// <summary>
    /// The actual SQL Query to execute
    /// </summary>
    public string QueryText { get; private set; }

    /// <summary>
    /// The metadata for the source table
    /// </summary>
    public OOAdvantech.MetaDataRepository.Classifier SourceMetadata { get; set; }

    /// <summary>
    /// The function to execute in order to perform projection
    /// </summary>
    public LambdaExpression LambdaExpression { get; set; }

    /// <summary>
    /// What exactly will this result return?
    /// Is it a sequence of entities or just one entity?
    /// </summary>
    public ResultShape ResultShape { get; private set; }

    /// <summary>
    /// When using FirstOrDefault or SingleOrDefault this is set to true
    /// telling the mapper that it is OK if no results are returned and
    /// that it should use null
    /// </summary>
    public bool UseDefault { get; set; }
  }

  internal enum ResultShape
  {
    None,       // it means that the query is not expected to have a return value
    Singleton,  // it returns a single element
    Sequence    // it returns a sequence of elements
  }
}