using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Retribution
{
    class Selector
    {
        Map currentLevel;
        Rectangle selectionSpace;
        Boolean isOccupied;
        Map nextLevel;
        Boolean isInteractable; // This is for the level selection nodes
        Boolean unlocked;

        // Once you beat a map, you return to the level selection screen and the next level is now interactable.

        public Selector(Rectangle space, Map current, Map next, Boolean interact)
        {
            this.selectionSpace = space;
            isOccupied = false;
            this.nextLevel = next;
            this.currentLevel = current;
            this.isInteractable = interact;
        }

        public Boolean getUnlocked()
        {
            return this.unlocked;
        }

        public void setUnlocked(Boolean unlock)
        {
            this.unlocked = unlock;
        }

        public void isColliding(GameObject unit)
        {
            if (this.selectionSpace.Intersects(unit.Bounds) && this.isInteractable == true)
            {
                this.setOccupied(true);
            }
            else this.setOccupied(false);
        }

        public Boolean getOccupied()
        {
            return this.isOccupied;
        }

        public void setOccupied(Boolean value)
        {
            this.isOccupied = value;
        }

        public void setInteraction(Boolean value)
        {
            this.isInteractable = value;
        }

        public Map getNext()
        {
            return this.nextLevel;
        }

        public Boolean getInteraction()
        {
            return this.isInteractable;
        }
    }
}
