using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardware;

namespace Firmware
{
    public class Z_RailFunction
    {
        PrinterControl printer;

        private double currentheightposition;
        private bool initialized;

        public Z_RailFunction(PrinterControl Printer)
        {
            printer = Printer;
            initialized = false;

        }
        //set height after z is sent from firmware
        //states for each 3 states
        //calls step stepper afer the calucaltion of steps

        //public static bool initialized = false; //states...unknown- known 
        //private current hieght postion variable



        //initialize method
        //checks state and initializes...then sets height
        public void InitializeState()
        {
            printer.ResetStepper();

            if (initialized == false)
            {
                GoToLimitSwitch();
                GoToBuildSurface();
            }
            initialized = true;
            currentheightposition = 0;

        }
        public void GoToBuildSurface()
        {
            double currentDelay = 625;
            double microsecondsPassed = 0;
            for (int i = 0; i < 40000; i++)
            {
                printer.StepStepper(PrinterControl.StepperDir.STEP_DOWN);
                printer.WaitMicroseconds((long)currentDelay);
                microsecondsPassed = microsecondsPassed + currentDelay;
                if (microsecondsPassed > 1000000L)
                {
                    currentDelay = currentDelay - 100;
                    if (currentDelay < 150) currentDelay = 150;
                    microsecondsPassed = 0;
                }
            }

        }
        public void GoToLimitSwitch()
        {
            double currentDelay = 625;
            double microsecondsPassed = 0;
            while (printer.LimitSwitchPressed() == false)
            {
                printer.StepStepper(PrinterControl.StepperDir.STEP_UP);

                printer.WaitMicroseconds((long)currentDelay);
                microsecondsPassed = microsecondsPassed + currentDelay;

                if (microsecondsPassed > 1000000L)
                {
                    currentDelay = currentDelay - 60;
                    if (currentDelay < 70) currentDelay = 70;
                    microsecondsPassed = 0;
                }
            }
        }
        public void SetHeight(float height)
        {
            InitializeState();
            double real_height = (double)height - currentheightposition;
            double num_steps = real_height * 400;
            for (int i = 0; i < num_steps; i++)
            {
                printer.StepStepper(PrinterControl.StepperDir.STEP_UP);
                printer.WaitMicroseconds(625);
            }
            currentheightposition = currentheightposition + height;

        }

    }
}
