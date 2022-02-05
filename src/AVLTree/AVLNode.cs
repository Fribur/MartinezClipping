
namespace Martinez
    {

public class AVLNode<T>
{
    public AVLNode<T> Left     { get; set; }
    public AVLNode<T> Right    { get; set; }
    public AVLNode<T> Parent   { get; set; }

    public T Value             { get; set; }
    public int Height          { get; set; }

    public AVLNode(T value)
    {
        this.Value = value;

        this.Parent  = null;
        this.Left    = null;
        this.Right   = null;

        this.Height  = 1;
    }

    public AVLNode<T> GetPredecessor()
    {
        if (Left != null)
        {
            return Left.GetFarRight();
        }
        else
        {
            AVLNode<T> p = this;

            while (p.Parent != null && p.Parent.Left == p)
            {
                p = p.Parent;
            }

            return p.Parent;
        }
    }

    public AVLNode<T> GetSuccessor()
    {
        if (Right != null)
        {
            return Right.GetFarLeft();
        }
        else
        {
            AVLNode<T> p = this;

            while (p.Parent != null && p.Parent.Right == p)
            {
                p = p.Parent;
            }

            return p.Parent;
        }
    }

    public AVLNode<T> GetFarLeft()
    {
        AVLNode<T> result = this;

        while (result.Left != null)
        {
            result = result.Left;
        }

        return result;
    }

    public AVLNode<T> GetFarRight()
    {
        AVLNode<T> result = this;

        while (result.Right != null)
        {
            result = result.Right;
        }

        return result;
    }
}

} 


