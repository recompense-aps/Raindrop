using Godot;
using System;

public class MenuItem : Container
{
    private Label _nameText;
    private Label _costLabel;
    private Label _descriptionLabel;
    public override void _Ready()
    {
        Ready_FindLabels();
    }
    public override void _Process(float delta)
    {
        
    }
    public void SetDetails(string name, int cost, string description)
    {
        _nameText.Text = name;
        _costLabel.Text = cost.ToString();
        _descriptionLabel.Text = description;
    }
    private void Ready_FindLabels()
    {
        _nameText = FindNode("Name") as Label;
        _costLabel = FindNode("Cost") as Label;
        _descriptionLabel = FindNode("Description") as Label;
    }
}
