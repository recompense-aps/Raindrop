﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace RainDrop
{
    class PickBag<T>
    {
        RandomNumberGenerator _rand = new RandomNumberGenerator();
        Dictionary<int[], T> _bag = new Dictionary<int[], T>();
        int _index = 1;

        public void Add(int chance, T choice)
        {
            if (_index > 100)
            {
                throw new Exception("bag is full!");
            }

            _bag.Add(new int[] { _index, _index + chance }, choice);
            _index += chance;
        }

        public T Pick()
        {
            _rand.Randomize();
            int r = _rand.RandiRange(1, 100);
            foreach (int[] key in _bag.Keys)
            {
                if (r >= key[0] && r < key[1])
                {
                    return _bag[key];
                }
            }
            throw new Exception("uh oh, bag broke");
        }
    }
}