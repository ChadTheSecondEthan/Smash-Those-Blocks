public class WeightBlock : Block
{
    float otherBlockMass;
    float otherThrowPower;

    public override void OnBecomeFusedTo(Block other)
    {
        otherBlockMass = other.rigidbody.mass;
        otherThrowPower = other.throwPower;
        other.rigidbody.mass = rigidbody.mass;
        other.throwPower = throwPower;
    }

    public override void OnBreakApartFrom(Block other)
    {
        other.rigidbody.mass = otherBlockMass;
        other.throwPower = otherThrowPower;
    }

    protected override bool Fusable(Block other) => !other.ContainsBlockOfType<WeightBlock>();
}
